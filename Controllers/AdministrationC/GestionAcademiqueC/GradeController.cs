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
        public class GradeController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public GradeController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Grade>>> GetGrades()
            {
                try
                {
                    var grades = await _context.Grades.ToListAsync();
                    if (grades == null || grades.Count == 0)
                    {
                        return NotFound(new { message = "Aucun grade trouvé." });
                    }
                    return Ok(grades);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Grade>> GetGrade(int id)
            {
                try
                {
                    var grade = await _context.Grades.FindAsync(id);
                    if (grade == null)
                    {
                        return NotFound(new { message = "Grade non trouvé." });
                    }
                    return Ok(grade);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPost]
            public async Task<ActionResult<Grade>> PostGrade(Grade grade)
            {
                try
                {
                    if (grade == null)
                    {
                        return BadRequest(new { message = "Les données du grade sont invalides." });
                    }

                    _context.Grades.Add(grade);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetGrade), new { id = grade.Id }, grade);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutGrade(int id, Grade grade)
            {
                try
                {
                    if (id != grade.Id)
                    {
                        return BadRequest(new { message = "Les IDs ne correspondent pas." });
                    }

                    _context.Entry(grade).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeExists(id))
                    {
                        return NotFound(new { message = "Grade non trouvé pour la mise à jour." });
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteGrade(int id)
            {
                try
                {
                    var grade = await _context.Grades.FindAsync(id);
                    if (grade == null)
                    {
                        return NotFound(new { message = "Grade non trouvé." });
                    }

                    _context.Grades.Remove(grade);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            private bool GradeExists(int id)
            {
                return _context.Grades.Any(e => e.Id == id);
            }
        }
    }

}
