using m_motors_API.Data;
using m_motors_API.DTO;
using m_motors_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace m_motors_API.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly MMotorsContext _context;
        private readonly IWebHostEnvironment _env;

        public DocumentsController(MMotorsContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Upload des documents
        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] UploadDocumentDto model)
        {
            try
            {
                if (model == null || model.File == null || model.File.Length == 0)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Fichier invalide"
                    });
                }

                var dossier = await _context.Dossiers
                    .FirstOrDefaultAsync(d => d.IdDossier == model.DossierId);

                if (dossier == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Dossier introuvable"
                    });
                }

                // Vérifie les doublons
                var existing = await _context.Documents
                    .FirstOrDefaultAsync(d =>
                        d.DossierId == model.DossierId &&
                        d.TypeDocument == model.TypeDocument);

                if (existing != null)
                {
                    _context.Documents.Remove(existing);
                }

                // dossier wwwroot sécurisé
                var webRoot = _env.WebRootPath;
                if (string.IsNullOrEmpty(webRoot))
                {
                    webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                var uploadFolder = Path.Combine(webRoot, "uploads", "documents");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // nom unique
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);

                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                // sauvegarde en BDD
                var document = new DocumentClient
                {
                    DossierId = model.DossierId,
                    TypeDocument = model.TypeDocument,
                    NomDocument = model.File.FileName,
                    CheminFichier = $"uploads/documents/{fileName}",
                    DateAjout = DateTime.Now
                };

                _context.Documents.Add(document);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Document uploadé avec succès"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erreur serveur",
                    error = ex.Message
                });
            }
        }

        // Vérifier si le dossier est complet
        [HttpGet("verifier/{dossierId}")]
        public IActionResult VerifierDossier(int dossierId)
        {
            var documents = _context.Documents
                .Where(d => d.DossierId == dossierId)
                .Select(d => d.TypeDocument)
                .ToList();

            var requis = new List<string>
            {
                "identite",
                "domicile",
                "revenus",
                "rib"
            };

            var manquants = requis.Except(documents).ToList();

            return Ok(new
            {
                complet = !manquants.Any(),
                manquants
            });
        }

        // Liste des documents d'un dossier
        [HttpGet("dossier/{dossierId}")]
        public IActionResult GetDocumentsByDossier(int dossierId)
        {
            var documents = _context.Documents
                .Where(d => d.DossierId == dossierId)
                .Select(d => new
                {
                    d.IdDocument,
                    d.NomDocument,
                    d.TypeDocument,
                    d.DateAjout,
                    d.CheminFichier
                })
                .ToList();

            return Ok(documents);
        }
    }
}
