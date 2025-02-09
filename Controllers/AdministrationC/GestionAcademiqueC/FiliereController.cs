namespace gradeManagerServerAPi.Controllers.AdministrationC.GestionAcademiqueC
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using global::gradeManagerServerAPi.Data.UserManagementAPI.Data;
    using global::gradeManagerServerAPi.Models.AdministrationM.GestionAcademique;

    namespace gradeManagerServerAPi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class FiliereController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public FiliereController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Filiere>>> GetFilieres()
            {
                try
                {
                    var filieres = await _context.Filieres.ToListAsync();
                    if (filieres == null || filieres.Count == 0)
                    {
                        return NotFound(new { message = "Aucune filière trouvée." });
                    }
                    return Ok(filieres);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Filiere>> GetFiliere(int id)
            {
                try
                {
                    var filiere = await _context.Filieres.FindAsync(id);
                    if (filiere == null)
                    {
                        return NotFound(new { message = "Filière non trouvée." });
                    }
                    return Ok(filiere);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPost]
            public async Task<ActionResult<Filiere>> PostFiliere(Filiere filiere)
            {
                try
                {
                    if (filiere == null)
                    {
                        return BadRequest(new { message = "Les données de la filière sont invalides." });
                    }

                    _context.Filieres.Add(filiere);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetFiliere), new { id = filiere.Id }, filiere);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutFiliere(int id, Filiere filiere)
            {
                try
                {
                    if (id != filiere.Id)
                    {
                        return BadRequest(new { message = "Les IDs ne correspondent pas." });
                    }

                    _context.Entry(filiere).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FiliereExists(id))
                    {
                        return NotFound(new { message = "Filière non trouvée pour la mise à jour." });
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteFiliere(int id)
            {
                try
                {
                    var filiere = await _context.Filieres.FindAsync(id);
                    if (filiere == null)
                    {
                        return NotFound(new { message = "Filière non trouvée." });
                    }

                    _context.Filieres.Remove(filiere);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            private bool FiliereExists(int id)
            {
                return _context.Filieres.Any(e => e.Id == id);
            }
        }
    }

}
