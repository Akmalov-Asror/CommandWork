using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;

namespace TestProject.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class AuditController : ControllerBase
{
    private readonly AppDbContext _context;
    public AuditController(AppDbContext context) => _context = context;
    // GET: api/Audit/GetAllAudits
    [HttpGet]
    public async Task<IActionResult> GetAllAudits() => Ok(await _context.AuditLog.ToListAsync());
}