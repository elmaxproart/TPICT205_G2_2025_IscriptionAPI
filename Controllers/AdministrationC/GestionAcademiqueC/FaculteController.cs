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
        public class FaculteController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public FaculteController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Faculte>>> GetFacultes()
            {
                try
                {
                    var facultes = await _context.Facultes.ToListAsync();
                    if (facultes == null || facultes.Count == 0)
                    {
                        return NotFound(new { message = "Aucune faculté trouvée." });
                    }
                    return Ok(facultes);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Faculte>> GetFaculte(int id)
            {
                try
                {
                    var faculte = await _context.Facultes.FindAsync(id);
                    if (faculte == null)
                    {
                        return NotFound(new { message = "Faculté non trouvée." });
                    }
                    return Ok(faculte);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPost]
            public async Task<ActionResult<Faculte>> PostFaculte(Faculte faculte)
            {
                try
                {
                    if (faculte == null)
                    {
                        return BadRequest(new { message = "Les données de la faculté sont invalides." });
                    }

                    _context.Facultes.Add(faculte);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetFaculte), new { id = faculte.Id }, faculte);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutFaculte(int id, Faculte faculte)
            {
                try
                {
                    if (id != faculte.Id)
                    {
                        return BadRequest(new { message = "Les IDs ne correspondent pas." });
                    }

                    _context.Entry(faculte).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaculteExists(id))
                    {
                        return NotFound(new { message = "Faculté non trouvée pour la mise à jour." });
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteFaculte(int id)
            {
                try
                {
                    var faculte = await _context.Facultes.FindAsync(id);
                    if (faculte == null)
                    {
                        return NotFound(new { message = "Faculté non trouvée." });
                    }

                    _context.Facultes.Remove(faculte);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            private bool FaculteExists(int id)
            {
                return _context.Facultes.Any(e => e.Id == id);
            }
        }
    }

}
