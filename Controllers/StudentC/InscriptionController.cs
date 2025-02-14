namespace gradeManagerServerAPi.Controllers.StudentC
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using global::gradeManagerServerAPi.Data.UserManagementAPI.Data;
    using global::gradeManagerServerAPi.Models.StudentM;

    using System.IO;
    using PdfSharpCore.Drawing;
    using PdfSharpCore.Pdf;
    using Microsoft.AspNetCore.Http.HttpResults;

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
            [HttpGet("etudiants-payes")]
            public async Task<ActionResult<IEnumerable<Etudiant>>> GetEtudiantsAyantPaye()
            {
                try
                {
                    // Récupérer les étudiants ayant des paiements valides  
                    var etudiantsAyantPaye = await _context.Etudiants
                        .Include(e => e.Paiements) // Inclure les paiements pour une éventuelle validation supplémentaire  
                        .Where(e => e.Paiements.Any(p => p.Valide)) // Filtrer uniquement les étudiants avec des paiements valides  
                        .ToListAsync();

                    // Vérifier s'il n'y a pas d'étudiants trouvés  
                    if (!etudiantsAyantPaye.Any())
                    {
                        return NotFound(new { message = "Aucun étudiant n'a effectué de paiement valide." });
                    }

                    return Ok(etudiantsAyantPaye);
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

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.Matricule == inscription.Matricule);
                    if (etudiant == null)
                    {
                        return BadRequest(new { message = "L'étudiant n'existe pas ou ne fait partie d'aucune classe." });
                    }

                    // Vérifier que les UE existent bien  
                    var ues = await _context.UEs.Where(ue => inscription.UeIds.Contains(ue.Id)).ToListAsync();
                    if (ues.Count != inscription.UeIds.Count)
                    {
                        return BadRequest(new { message = "Une ou plusieurs UE sélectionnées n'existent pas." });
                    }

                    // Vérifier si l'étudiant a effectué un paiement valide  
                    var paiement = await _context.Paiements
                        .FirstOrDefaultAsync(p => p.EtudiantId == etudiant.Id && p.Valide);
                    if (paiement == null)
                    {
                        return BadRequest(new { message = "L'étudiant n'a pas effectué de paiement valide." });
                    }

                    // Vérifier si l'étudiant est déjà inscrit  
                    var inscriptionExistante = await _context.Inscriptions
                        .FirstOrDefaultAsync(i => i.EtudiantId == etudiant.Id && i.EstValide);
                    if (inscriptionExistante != null)
                    {
                        return BadRequest(new { message = "L'étudiant est déjà inscrit." });
                    }

                    // Associer l'étudiant et les UE à l'inscription  
                    inscription.EtudiantId = etudiant.Id;
                    inscription.Etudiant = etudiant;
                    inscription.Ues = ues;
                    inscription.DateInscription = DateTime.Now;
                    inscription.EstValide = true;
                    inscription.semestre = 1;

                    _context.Inscriptions.Add(inscription);
                    await _context.SaveChangesAsync();

                    // Générer le PDF de l'inscription après l'enregistrement
                    var pdfResult = await GeneratePdf(inscription.Etudiant.Matricule);

                    // Vous pouvez choisir de retourner le fichier PDF directement ou un lien vers le PDF
                    // Exemple : retourner le fichier PDF généré
                    return File(pdfResult.FileContents, "application/pdf", pdfResult.FileName);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }

            private async Task<(byte[] FileContents, string FileName)> GeneratePdf(string matricule)
            {
                // Recherche de l'inscription basée sur le matricule de l'étudiant
                var inscription = await _context.Inscriptions
                    .Include(i => i.Etudiant)
                    .Include(i => i.Ues)
                    .FirstOrDefaultAsync(i => i.Etudiant.Matricule == matricule);

                if (inscription == null || inscription.Etudiant == null)
                {
                    throw new Exception("Inscription ou étudiant non trouvé.");
                }

                // Spécifier le répertoire où vous souhaitez enregistrer le PDF
                string directoryPath = @"C:\Temp"; // Remplacez ce chemin par votre propre répertoire

                // Créer le répertoire si nécessaire
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string fileName = $"Inscription_{inscription.Etudiant.Matricule}.pdf";
                string filePath = Path.Combine(directoryPath, fileName);

                // Création du document PDF
                using (var memoryStream = new MemoryStream())
                {
                    var document = new PdfDocument();
                    var page = document.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);

                    // Fonts
                    var titleFont = new XFont("Arial", 16, XFontStyle.Bold);
                    var headerFont = new XFont("Arial", 12, XFontStyle.Bold);
                    var bodyFont = new XFont("Arial", 12, XFontStyle.Regular);
                    var fieldFont = new XFont("Arial", 12, XFontStyle.BoldItalic);
                    int yPos = 50;

                    // Couleurs
                    XBrush titleColor = XBrushes.DarkBlue;
                    XBrush fieldColor = XBrushes.Gray;
                    XBrush valueColor = XBrushes.Black;
                    XPen lineColor = XPens.LightGray;

                    // Calculer la position X pour centrer le texte
                    double textWidth = gfx.MeasureString("Université Yaoundé 1", titleFont).Width;
                    double xPos = (page.Width - textWidth) / 2;

                    // Dessiner le texte centré
                    gfx.DrawString("Université Yaoundé 1", titleFont, titleColor, new XPoint(xPos, yPos));


                    yPos += 30;
                    gfx.DrawString("Faculté des Sciences", fieldFont, fieldColor, new XPoint(40, yPos));
                    gfx.DrawString("Faculty of Science", fieldFont, fieldColor, new XPoint(page.Width - 200, yPos));

                    yPos += 20; // Espacement plus petit entre les lignes

                    gfx.DrawString("Semestre: " + inscription.semestre, fieldFont, fieldColor, new XPoint(40, yPos));
                    gfx.DrawString("Semester: " + inscription.semestre, fieldFont, fieldColor, new XPoint(page.Width - 200, yPos));

                    yPos += 20; // Espacement entre les autres lignes

                    gfx.DrawString("Département: d'informatique", fieldFont, fieldColor, new XPoint(40, yPos));
                    gfx.DrawString("Department: Computer Science", fieldFont, fieldColor, new XPoint(page.Width - 200, yPos));

                    yPos += 1;

                    XImage image = XImage.FromFile(@"icon-192.png"); 

                    
                    double logoWidthInPoints = 100 * 72 / 96;  
                    double logoHeightInPoints = 100 * 72 / 96; 

                    
                    double imageX = (page.Width - logoWidthInPoints) / 2;
                    double imageY = yPos;

                   
                    gfx.DrawImage(image, imageX, imageY, logoWidthInPoints, logoHeightInPoints);

                    
                    yPos += (int)(logoHeightInPoints + 20);



                    yPos += 50;
                    

                    gfx.DrawString("Fiche d'Inscription ", titleFont, titleColor, new XPoint(xPos, yPos));
                    yPos += 20;
                    gfx.DrawString("2024-2025 ", titleFont, titleColor, new XPoint(xPos, yPos));
                    yPos += 40;

                    gfx.DrawLine(lineColor, 40, yPos, page.Width - 40, yPos);
                    yPos += 30;

                    // Informations de l'étudiant - Champs à gauche, Valeurs à droite
                    gfx.DrawString("Matricule:", fieldFont, fieldColor, new XPoint(40, yPos));
                    gfx.DrawString(inscription.Etudiant.Matricule, bodyFont, valueColor, new XPoint(page.Width - 200, yPos));
                    yPos += 20;

                    gfx.DrawString("Nom:", fieldFont, fieldColor, new XPoint(40, yPos));
                    gfx.DrawString($"{inscription.Etudiant.Nom} {inscription.Etudiant.Prenom}", bodyFont, valueColor, new XPoint(page.Width - 200, yPos));
                    yPos += 20;

                    gfx.DrawString("Téléphone:", fieldFont, fieldColor, new XPoint(40, yPos));
                    gfx.DrawString(inscription.Etudiant.Numero.ToString(), bodyFont, valueColor, new XPoint(page.Width - 200, yPos));
                    yPos += 20;

                    gfx.DrawString("Email:", fieldFont, fieldColor, new XPoint(40, yPos));
                    gfx.DrawString(inscription.Etudiant.Email, bodyFont, valueColor, new XPoint(page.Width - 200, yPos));
                    yPos += 20;

                    gfx.DrawString("Date d'inscription:", fieldFont, fieldColor, new XPoint(40, yPos));
                    gfx.DrawString(inscription.DateInscription.ToString("dd/MM/yyyy"), bodyFont, valueColor, new XPoint(page.Width - 200, yPos));
                    yPos += 30;

                    // Ligne de séparation
                    gfx.DrawLine(lineColor, 40, yPos, page.Width - 40, yPos);
                    yPos += 30;

                    // Informations sur les Unités d'Enseignement (UE) sous forme de tableau
                    if (inscription.Ues == null || !inscription.Ues.Any())
                    {
                        gfx.DrawString("Aucune Unité d'Enseignement sélectionnée.", bodyFont, valueColor, new XPoint(40, yPos));
                    }
                    else
                    {
                        gfx.DrawString("Unités d'Enseignement sélectionnées :", bodyFont, valueColor, new XPoint(40, yPos));
                        yPos += 20;

                        // Dessiner le tableau des UE
                        double tableStartY = yPos;
                        gfx.DrawString("Libellé", headerFont, XBrushes.DarkGreen, new XPoint(40, tableStartY));
                        gfx.DrawString("Semestre", headerFont, XBrushes.DarkGreen, new XPoint(page.Width - 180, tableStartY));
                        yPos += 20;

                        foreach (var ue in inscription.Ues)
                        {
                            gfx.DrawString(ue.Libelle, bodyFont, valueColor, new XPoint(40, yPos));
                            gfx.DrawString($"Semestre {ue.Semestre}", bodyFont, valueColor, new XPoint(page.Width - 180, yPos));
                            yPos += 20;
                        }
                    }

                    yPos += 30;

                    // Ligne de séparation
                    gfx.DrawLine(lineColor, 40, yPos, page.Width - 40, yPos);
                    yPos += 10;

                    yPos += 150;
                    // Signature en bas à droite
                    gfx.DrawString("Signature de l'étudiant : ____________________", bodyFont, valueColor, new XPoint(page.Width - 250, yPos));

                    // Sauvegarder le fichier PDF dans un MemoryStream
                    document.Save(memoryStream);
                    memoryStream.Position = 0; // Rewind le MemoryStream avant de le lire

                    // Retourner le fichier PDF généré pour téléchargement
                    return (memoryStream.ToArray(), fileName);
                }
            }


            [HttpGet("fiche-pdf/{matricule}")]
            public async Task<IActionResult> Generate(string matricule)
            {
                try
                {
                    // Recherche de l'inscription basée sur le matricule de l'étudiant
                    var inscription = await _context.Inscriptions
                        .Include(i => i.Etudiant)
                        .Include(i => i.Ues)
                        .FirstOrDefaultAsync(i => i.Etudiant.Matricule == matricule);

                    // Vérifier si l'inscription existe
                    if (inscription == null)
                    {
                        return NotFound(new { message = "Inscription non trouvée." });
                    }

                    // Vérifier si l'étudiant associé existe
                    if (inscription.Etudiant == null)
                    {
                        return BadRequest(new { message = "L'étudiant associé à cette inscription est introuvable." });
                    }

                    // Générer le PDF
                    var pdfResult = await GeneratePdf(matricule);

                    // Retourner le fichier PDF généré
                    return File(pdfResult.FileContents, "application/pdf", pdfResult.FileName);
                }
                catch (Exception ex)
                {
                    // Log l'erreur générale (vous pouvez aussi logger dans un fichier ou un service)
                    Console.WriteLine($"Erreur lors de la génération du PDF: {ex.Message}");
                    return StatusCode(500, new { message = $"Erreur lors de la génération du PDF: {ex.Message}" });
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
           


           
            
            private bool InscriptionExists(int id)
            {
                return _context.Inscriptions.Any(e => e.Id == id);
            }
        }
    }

}
