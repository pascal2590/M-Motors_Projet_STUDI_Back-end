using Microsoft.AspNetCore.Mvc;
using m_motors_API.DTO;

namespace m_motors_API.Controllers
{
    [ApiController]
    [Route("api/dossiers")]
    public class DossiersController : ControllerBase
    {
        // -----------------------------
        // ACHAT
        // -----------------------------
        [HttpPost("achat")]
        public IActionResult CreateAchat([FromBody] DossierAchatDto dto)
        {
            if (dto == null)
                return BadRequest();

            return Ok(new
            {
                message = "Dossier achat reçu",
                data = dto
            });
        }

        // -----------------------------
        // LLD
        // -----------------------------
        [HttpPost("lld")]
        public IActionResult CreateLld([FromBody] DossierLldDto dto)
        {
            if (dto == null)
                return BadRequest();

            return Ok(new
            {
                message = "Dossier LLD reçu",
                data = dto
            });
        }
    }
}
