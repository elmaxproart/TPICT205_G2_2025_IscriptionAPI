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
        public class NiveauController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public NiveauController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Niveau>>> GetNiveaux()
            {
                try
                {
                    var niveaux = await _context.Niveaux.ToListAsync();
                    if (niveaux == null || niveaux.Count == 0)
                    {
                        return NotFound(new { message = "Aucun niveau trouvé." });
                    }
                    return Ok(niveaux);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Niveau>> GetNiveau(int id)
            {
                try
                {
                    var niveau = await _context.Niveaux.FindAsync(id);
                    if (niveau == null)
                    {
                        return NotFound(new { message = "Niveau non trouvé." });
                    }
                    return Ok(niveau);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPost]
            public async Task<ActionResult<Niveau>> PostNiveau(Niveau niveau)
            {
                try
                {
                    if (niveau == null)
                    {
                        return BadRequest(new { message = "Les données du niveau sont invalides." });
                    }

                    _context.Niveaux.Add(niveau);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetNiveau), new { id = niveau.Id }, niveau);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutNiveau(int id, Niveau niveau)
            {
                try
                {
                    if (id != niveau.Id)
                    {
                        return BadRequest(new { message = "Les IDs ne correspondent pas." });
                    }

                    _context.Entry(niveau).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NiveauExists(id))
                    {
                        return NotFound(new { message = "Niveau non trouvé pour la mise à jour." });
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteNiveau(int id)
            {
                try
                {
                    var niveau = await _context.Niveaux.FindAsync(id);
                    if (niveau == null)
                    {
                        return NotFound(new { message = "Niveau non trouvé." });
                    }

                    _context.Niveaux.Remove(niveau);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            private bool NiveauExists(int id)
            {
                return _context.Niveaux.Any(e => e.Id == id);
            }
        }
    }

}
