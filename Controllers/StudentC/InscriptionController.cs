namespace gradeManagerServerAPi.Controllers.StudentC
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using global::gradeManagerServerAPi.Data.UserManagementAPI.Data;
    using global::gradeManagerServerAPi.Models.StudentM;
    using iText.Kernel.Pdf;
    using iText.Layout.Element;
    using iText.Layout;

    namespace gradeManagerServerAPi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class InscriptionController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public InscriptionController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Inscription>>> GetInscriptions()
            {
                try
                {
                    var inscriptions = await _context.Inscriptions.ToListAsync();
                    if (inscriptions == null || inscriptions.Count == 0)
                    {
                        return NotFound(new { message = "Aucune inscription trouvée." });
                    }
                    return Ok(inscriptions);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Inscription>> GetInscription(int id)
            {
                try
                {
                    var inscription = await _context.Inscriptions.FindAsync(id);
                    if (inscription == null)
                    {
                        return NotFound(new { message = "Inscription non trouvée." });
                    }
                    return Ok(inscription);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPost]
            public async Task<ActionResult<Inscription>> PostInscription(Inscription inscription)
            {
                try
                {
                    if (inscription == null)
                    {
                        return BadRequest(new { message = "Les données d'inscription sont invalides." });
                    }

                    _context.Inscriptions.Add(inscription);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetInscription), new { id = inscription.Id }, inscription);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutInscription(int id, Inscription inscription)
            {
                try
                {
                    if (id != inscription.Id)
                    {
                        return BadRequest(new { message = "Les IDs ne correspondent pas." });
                    }

                    _context.Entry(inscription).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscriptionExists(id))
                    {
                        return NotFound(new { message = "Inscription non trouvée pour la mise à jour." });
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteInscription(int id)
            {
                try
                {
                    var inscription = await _context.Inscriptions.FindAsync(id);
                    if (inscription == null)
                    {
                        return NotFound(new { message = "Inscription non trouvée." });
                    }

                    _context.Inscriptions.Remove(inscription);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }
            // POST: api/Inscription
            [HttpPost("/fiche-inscription-pdf")]
            public async Task<ActionResult<Inscription>> PostInscriptionAndGeneRatePdf(Inscription inscription)
            {
                try
                {
                    if (inscription == null)
                    {
                        return BadRequest(new { message = "Les données d'inscription sont invalides." });
                    }

                    _context.Inscriptions.Add(inscription);
                    await _context.SaveChangesAsync();

                    // Après avoir créé l'inscription, génère le PDF
                    var pdfBytes = GeneratePdf(inscription);

                    // Retourner le PDF généré en réponse
                    return File(pdfBytes, "application/pdf", $"Fiche_Inscription_{inscription.Id}.pdf");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            // Générer le PDF à partir des données de l'inscription
            private byte[] GeneratePdf(Inscription inscription)
            {
                using (var ms = new MemoryStream())
                {
                    using (var writer = new PdfWriter(ms))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            var document = new Document(pdf);

                            // Ajouter des éléments au PDF
                            Document inscriptionDocument = document.Add(new Paragraph("Fiche d'Inscription").SetFontSize(20));
                            document.Add(new Paragraph($"ID: {inscription.Id}"));
                            document.Add(new Paragraph($"Nom: {inscription.Etudiant.Nom}"));
                          
                            document.Add(new Paragraph($"Date de Naissance: {inscription.Etudiant.DateNaissance:dd/MM/yyyy}"));
                            document.Add(new Paragraph($"Email: {inscription.Etudiant.Email}"));
                            document.Add(new Paragraph($"Filière: {inscription.Classe.Filiere}"));
                            document.Add(new Paragraph($"Date d'Inscription: {inscription.DateInscription:dd/MM/yyyy}"));

                           
                        }
                    }

                    return ms.ToArray();
                }
            }
            private bool InscriptionExists(int id)
            {
                return _context.Inscriptions.Any(e => e.Id == id);
            }
        }
    }

}
