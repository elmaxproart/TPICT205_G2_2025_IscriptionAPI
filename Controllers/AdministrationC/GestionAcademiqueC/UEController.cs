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
        public class UEController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public UEController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<UE>>> GetUEs()
            {
                try
                {
                    var ues = await _context.UEs.ToListAsync();
                    if (ues == null || ues.Count == 0)
                    {
                        return NotFound(new { message = "Aucune UE trouvée." });
                    }
                    return Ok(ues);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<UE>> GetUE(int id)
            {
                try
                {
                    var ue = await _context.UEs.FindAsync(id);
                    if (ue == null)
                    {
                        return NotFound(new { message = "UE non trouvée." });
                    }
                    return Ok(ue);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPost]
            public async Task<ActionResult<UE>> PostUE(UE ue)
            {
                try
                {
                    if (ue == null)
                    {
                        return BadRequest(new { message = "Les données de l'UE sont invalides." });
                    }

                    _context.UEs.Add(ue);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetUE), new { id = ue.Id }, ue);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutUE(int id, UE ue)
            {
                try
                {
                    if (id != ue.Id)
                    {
                        return BadRequest(new { message = "Les IDs ne correspondent pas." });
                    }

                    _context.Entry(ue).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UEExists(id))
                    {
                        return NotFound(new { message = "UE non trouvée pour la mise à jour." });
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUE(int id)
            {
                try
                {
                    var ue = await _context.UEs.FindAsync(id);
                    if (ue == null)
                    {
                        return NotFound(new { message = "UE non trouvée." });
                    }

                    _context.UEs.Remove(ue);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            private bool UEExists(int id)
            {
                return _context.UEs.Any(e => e.Id == id);
            }
        }
    }

}
