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
        public class DepartementController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public DepartementController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Departement>>> GetDepartements()
            {
                try
                {
                    var departements = await _context.Departements.ToListAsync();
                    if (departements == null || departements.Count == 0)
                    {
                        return NotFound(new { message = "Aucun département trouvé." });
                    }
                    return Ok(departements);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Departement>> GetDepartement(int id)
            {
                try
                {
                    var departement = await _context.Departements.FindAsync(id);
                    if (departement == null)
                    {
                        return NotFound(new { message = "Département non trouvé." });
                    }
                    return Ok(departement);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPost]
            public async Task<ActionResult<Departement>> PostDepartement(Departement departement)
            {
                try
                {
                    if (departement == null)
                    {
                        return BadRequest(new { message = "Les données du département sont invalides." });
                    }

                    _context.Departements.Add(departement);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetDepartement), new { id = departement.Id }, departement);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutDepartement(int id, Departement departement)
            {
                try
                {
                    if (id != departement.Id)
                    {
                        return BadRequest(new { message = "Les IDs ne correspondent pas." });
                    }

                    _context.Entry(departement).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartementExists(id))
                    {
                        return NotFound(new { message = "Département non trouvé pour la mise à jour." });
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteDepartement(int id)
            {
                try
                {
                    var departement = await _context.Departements.FindAsync(id);
                    if (departement == null)
                    {
                        return NotFound(new { message = "Département non trouvé." });
                    }

                    _context.Departements.Remove(departement);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            private bool DepartementExists(int id)
            {
                return _context.Departements.Any(e => e.Id == id);
            }
        }
    }

}
