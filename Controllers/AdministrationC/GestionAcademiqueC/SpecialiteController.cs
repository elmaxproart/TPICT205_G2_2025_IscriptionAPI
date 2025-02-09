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
        public class SpecialiteController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public SpecialiteController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Specialite>>> GetSpecialites()
            {
                try
                {
                    var specialites = await _context.Specialites.ToListAsync();
                    if (specialites == null || specialites.Count == 0)
                    {
                        return NotFound(new { message = "Aucune spécialité trouvée." });
                    }
                    return Ok(specialites);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Specialite>> GetSpecialite(int id)
            {
                try
                {
                    var specialite = await _context.Specialites.FindAsync(id);
                    if (specialite == null)
                    {
                        return NotFound(new { message = "Spécialité non trouvée." });
                    }
                    return Ok(specialite);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPost]
            public async Task<ActionResult<Specialite>> PostSpecialite(Specialite specialite)
            {
                try
                {
                    if (specialite == null)
                    {
                        return BadRequest(new { message = "Les données de la spécialité sont invalides." });
                    }

                    _context.Specialites.Add(specialite);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetSpecialite), new { id = specialite.Id }, specialite);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutSpecialite(int id, Specialite specialite)
            {
                try
                {
                    if (id != specialite.Id)
                    {
                        return BadRequest(new { message = "Les IDs ne correspondent pas." });
                    }

                    _context.Entry(specialite).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialiteExists(id))
                    {
                        return NotFound(new { message = "Spécialité non trouvée pour la mise à jour." });
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteSpecialite(int id)
            {
                try
                {
                    var specialite = await _context.Specialites.FindAsync(id);
                    if (specialite == null)
                    {
                        return NotFound(new { message = "Spécialité non trouvée." });
                    }

                    _context.Specialites.Remove(specialite);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            private bool SpecialiteExists(int id)
            {
                return _context.Specialites.Any(e => e.Id == id);
            }
        }
    }

}
