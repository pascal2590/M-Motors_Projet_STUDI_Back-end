using Microsoft.AspNetCore.Mvc;
using m_motors_API.Data;
using m_motors_API.Models;

namespace m_motors_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly MMotorsContext _context;

        public ClientController(MMotorsContext context)
        {
            _context = context;
        }

        // POST: api/client
        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            client.DateUpload = DateTime.Now; // Date de création
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateClient), new { id = client.IdClient }, client);
        }
    }
}
