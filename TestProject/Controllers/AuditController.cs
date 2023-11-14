using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestProject.Domains;
using TestProject.Services.Implementation;
using TestProject.Services.Interfaces;

namespace TestProject.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class AuditController : ControllerBase
{
    private readonly IAuditRepository _context;
    public AuditController(IAuditRepository context) => _context = context;

    [HttpGet("List")]
    public async Task<IActionResult> GetAllAudits() => Ok(await _context.GetAllAudits());

    [HttpGet("Date")]
    [ProducesResponseType(typeof(List<AuditLog>), 200)]
    public async Task<IActionResult> GetFiltered(string? fromDate, string? toDate)
    {
        try
        {
            var result = await _context.GetFiltered(fromDate, toDate);
            return Ok(result);
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Name")]
    public async Task<IActionResult> SortByUserName(string name)
    {
        try
        {
            var result = await _context.SortByUserName(name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
    } 
}