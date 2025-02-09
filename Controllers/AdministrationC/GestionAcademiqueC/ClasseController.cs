namespace gradeManagerServerAPi.Controllers.AdministrationC.GestionAcademiqueC
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using global::gradeManagerServerAPi.Data.UserManagementAPI.Data;

    namespace gradeManagerServerAPi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ClasseController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public ClasseController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: api/Classe
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Classe>>> GetClasses()
            {
                try
                {
                    var classes = await _context.Classes.ToListAsync();
                    if (classes == null || classes.Count == 0)
                    {
                        return NotFound(new { message = "Aucune classe trouvée." });
                    }
                    return Ok(classes);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            // GET: api/Classe/{id}
            [HttpGet("{id}")]
            public async Task<ActionResult<Classe>> GetClasse(int id)
            {
                try
                {
                    var classe = await _context.Classes.FindAsync(id);

                    if (classe == null)
                    {
                        return NotFound(new { message = "Classe non trouvée." });
                    }

                    return Ok(classe);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            // POST: api/Classe
            [HttpPost]
            public async Task<ActionResult<Classe>> PostClasse(Classe classe)
            {
                try
                {
                    if (classe == null)
                    {
                        return BadRequest(new { message = "Les données de la classe sont invalides." });
                    }

                    _context.Classes.Add(classe);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetClasse), new { id = classe.Id }, classe);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            // PUT: api/Classe/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> PutClasse(int id, Classe classe)
            {
                try
                {
                    if (id != classe.Id)
                    {
                        return BadRequest(new { message = "Les IDs ne correspondent pas." });
                    }

                    _context.Entry(classe).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClasseExists(id))
                    {
                        return NotFound(new { message = "Classe non trouvée pour la mise à jour." });
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            // DELETE: api/Classe/{id}
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteClasse(int id)
            {
                try
                {
                    var classe = await _context.Classes.FindAsync(id);
                    if (classe == null)
                    {
                        return NotFound(new { message = "Classe non trouvée." });
                    }

                    _context.Classes.Remove(classe);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            private bool ClasseExists(int id)
            {
                return _context.Classes.Any(e => e.Id == id);
            }
        }
    }

}
