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
    // GET: api/Audit/GetAllAudits
    //[HttpGet]
    //public async Task<IActionResult> GetAllAudits() => Ok(await _context.AuditLog.ToListAsync());



    [HttpGet]
    [ProducesResponseType(typeof(List<AuditLog>), 200)]
    public async Task<IActionResult> GetFiltered(string? fromDate, string? toDate)
    {
        var dateFormat = "dd.MM.yyyy";  // Corrected date format      
    

        if (!DateTime.TryParseExact(fromDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDateParsed))
        {
            if (fromDate != null)
            {
                return BadRequest("Invalid fromDate format");
            }
        }

        if (!DateTime.TryParseExact(toDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDateParsed))
        {
            if (toDate != null)
            {
                return BadRequest("Invalid toDate format");
            }
        }

        fromDateParsed = DateTime.SpecifyKind(fromDateParsed, DateTimeKind.Utc);
        toDateParsed = DateTime.SpecifyKind(toDateParsed, DateTimeKind.Utc);

        // Convert fromDateParsed and toDateParsed to DateTime and do the filtering
        var auditLogs = await _context.AuditLog
            .Where(log =>
                (fromDateParsed == DateTime.MinValue || log.DateTime >= fromDateParsed) &&
                (toDateParsed == DateTime.MinValue || log.DateTime <= toDateParsed))
            .ToListAsync();

        return Ok(auditLogs);
    }





}