using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gradeManagerServerAPi.Data;
using gradeManagerServerAPi.Models.StudentM;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using gradeManagerServerAPi.Data.UserManagementAPI.Data;

namespace gradeManagerServerAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtudiantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EtudiantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET : api/Etudiants (Récupère tous les étudiants)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Etudiant>>> GetEtudiants()
        {
            try
            {
                return await _context.Etudiants.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne : {ex.Message}");
            }
        }

        // ✅ GET : api/Etudiants/{id} (Récupère un étudiant par ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Etudiant>> GetEtudiant(int id)
        {
            try
            {
                var etudiant = await _context.Etudiants.FindAsync(id);
                if (etudiant == null)
                {
                    return NotFound(new { message = "Étudiant introuvable." });
                }
                return etudiant;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne : {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Etudiant>> PostEtudiant([FromBody] Etudiant etudiant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // ✅ Vérifie si la classe existe en base avant d'affecter l'étudiant
                var classe = await _context.Classes.FindAsync(etudiant.ClasseId);
                if (classe == null)
                {
                    return NotFound("Classe introuvable");
                }

                // ✅ Associe la classe trouvée à l'étudiant
                etudiant.Classe = classe;

                _context.Etudiants.Add(etudiant);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEtudiant), new { id = etudiant.Id }, etudiant);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Erreur lors de l'ajout : {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne : {ex.Message}");
            }
        }
        // ✅ Recherche un étudiant par matricule et vérifie s'il a une classe
        [HttpGet("rechercher/{matricule}")]
        public async Task<ActionResult<Etudiant>> GetEtudiantByMatricule(string matricule)
        {
            
            var etudiant = await _context.Etudiants
                                         .Where(e => e.Matricule == matricule)
                                         .Include(e => e.Classe) 
                                         .FirstOrDefaultAsync();

            if (etudiant == null)
            {
                return NotFound($"Étudiant avec matricule {matricule} non trouvé.");
            }

            if (etudiant.Classe == null)
            {
                return NotFound($"L'étudiant avec matricule {matricule} n'a pas de classe attribuée.");
            }

            return Ok(etudiant);
        }
    

        // ✅ PUT : api/Etudiants/{id} (Met à jour un étudiant)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEtudiant(int id, [FromBody] Etudiant etudiant)
        {
            if (id != etudiant.Id)
            {
                return BadRequest(new { message = "L'ID ne correspond pas." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(etudiant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Etudiants.Any(e => e.Id == id))
                {
                    return NotFound(new { message = "Étudiant introuvable." });
                }
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Erreur lors de la mise à jour : {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne : {ex.Message}");
            }

            return NoContent();
        }

        // ✅ DELETE : api/Etudiants/{id} (Supprime un étudiant)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEtudiant(int id)
        {
            try
            {
                var etudiant = await _context.Etudiants.FindAsync(id);
                if (etudiant == null)
                {
                    return NotFound(new { message = "Étudiant introuvable." });
                }

                _context.Etudiants.Remove(etudiant);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Erreur lors de la suppression : {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne : {ex.Message}");
            }
        }
    }
}
