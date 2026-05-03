using Microsoft.AspNetCore.Mvc;
using m_motors_API.Data;

namespace m_motors_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesLldController : ControllerBase
    {
        private readonly MMotorsContext _context;

        public ServicesLldController(MMotorsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetServicesLLD()
        {
            var services = _context.ServiceLLDs
                .Select(s => new
                {
                    s.IdService,
                    s.NomService,
                    s.Description
                })
                .ToList();

            return Ok(services);
        }
    }    
}
