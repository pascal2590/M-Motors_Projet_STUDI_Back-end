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

        public DocumentsController(
            MMotorsContext context,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // UPLOAD DOCUMENT
        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(
            [FromForm] UploadDocumentDto model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return Ok(new
                {
                    success = false,
                    message = "Fichier invalide"
                });
            }

            // Vérifier dossier
            var dossier = await _context.Dossiers
                .FirstOrDefaultAsync(d =>
                    d.IdDossier == model.DossierId);

            if (dossier == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "Dossier introuvable"
                });
            }

            // Vérifier doublon
            var exists = await _context.Documents
                .AnyAsync(d =>
                    d.DossierId == model.DossierId &&
                    d.TypeDocument == model.TypeDocument);

            if (exists)
            {
                return Ok(new
                {
                    success = false,
                    message =
                        "Document déjà uploadé pour ce type"
                });
            }

            // Générer nom unique
            var uniqueFileName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(model.File.FileName);

            var uploadFolder =
                Path.Combine(
                    _env.WebRootPath,
                    "uploads",
                    "documents"
                );

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var filePath =
                Path.Combine(uploadFolder, uniqueFileName);

            // Sauvegarde physique
            using (var stream =
                   new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Sauvegarde base
            var document = new DocumentClient
            {
                NomDocument = model.File.FileName,
                TypeDocument = model.TypeDocument,
                CheminFichier =
                    $"uploads/documents/{uniqueFileName}",
                DateUpload = DateTime.Now,
                DossierId = model.DossierId
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Document uploadé"
            });
        }



        // VERIFIER DOSSIER COMPLET 
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

            var manquants =
                requis.Except(documents).ToList();

            return Ok(new
            {
                complet = !manquants.Any(),
                manquants
            });
        }

         // LISTE DOCUMENTS DOSSIER   
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
                    d.DateUpload
                })
                .ToList();

            return Ok(documents);
        }
    }
}
