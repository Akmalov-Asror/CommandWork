using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TestProject.Data;
using TestProject.Domains;

namespace TestProject.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class AuditController : ControllerBase
{
    private readonly AppDbContext _context;
    public AuditController(AppDbContext context) => _context = context;


    [HttpGet("List")]
    public async Task<IActionResult> GetAllAudits() => Ok(await _context.AuditLog.ToListAsync());

    [HttpGet("Date")]
    [ProducesResponseType(typeof(List<AuditLog>), 200)]
    public async Task<IActionResult> GetFiltered(string? fromDate, string? toDate)
    {
        var dateFormat = "dd.MM.yyyy";  
    

        if (!DateTime.TryParseExact(fromDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDateParsed))
        {
            if (fromDate != null)
            {
                return BadRequest("Invalid date format. fromDate For example : dd.mm.yyyy");
            }
        }

        if (!DateTime.TryParseExact(toDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDateParsed))
        {
            if (toDate != null)
            {
                return BadRequest("Invalid date format. toDate For example : dd.mm.yyyy");
            }
        }

        fromDateParsed = DateTime.SpecifyKind(fromDateParsed, DateTimeKind.Utc);
        toDateParsed = DateTime.SpecifyKind(toDateParsed, DateTimeKind.Utc);


        if (fromDateParsed.Date > toDateParsed.Date)
        {
            return BadRequest("To Date cannot be before From Date.");
        }

        var auditLogs = await _context.AuditLog
            .Where(log =>
                (fromDateParsed == DateTime.MinValue || log.DateTime >= fromDateParsed) &&
                (toDateParsed == DateTime.MinValue || log.DateTime <= toDateParsed))
            .ToListAsync();

        return Ok(auditLogs);
    }

    [HttpGet("Name")]
    public async Task<IActionResult> SortByUserName(string name)
    {
        var dataFromDatabase = await _context.AuditLog.Select(log => log.UserName).ToListAsync();

        var auditLogs = _context.AuditLog
            .AsEnumerable() 
            .Where(log => log.UserName.Equals(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(auditLogs);
    } 
}