using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Domains;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuditController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/<AuditController>
        [HttpGet]
        public async Task<IActionResult> GetAllAudits()
        {
            var audit = await _context.AuditLog.ToListAsync();
            if (audit == null)
            {
                return NotFound("Audit not found");
            }
            return Ok(audit);
        }

        

      
    }
}
