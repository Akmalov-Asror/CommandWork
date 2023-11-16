using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using TestProject.Domains;
using TestProject.Services.Interfaces;

namespace TestProject.Controllers;

[Route("api/[controller]")]
[ApiController]
    
public class AuditController : ControllerBase
{
    private readonly IAuditRepository _context;
    public AuditController(IAuditRepository context) => _context = context;
    /// <summary>
    /// Retrieves a list of all AuditLog items.
    /// </summary>
    /// <returns>
    ///     Returns an HTTP response containing a list of all AuditLog items:
    ///     - 200 OK: Returns a list of all AuditLog items available in the system.
    /// </returns>
    [HttpGet("List")]
    [ProducesResponseType(typeof(List<AuditLog>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAudits() => Ok(await _context.GetAllAudits());

    /// <summary>
    /// Retrieves a list of AuditLog items filtered by date range.
    /// </summary>
    /// <param name="fromDate">Start date of the filter range (optional).</param>
    /// <param name="toDate">End date of the filter range (optional).</param>
    /// <returns>
    ///     Returns an HTTP response containing a list of AuditLog items filtered by the specified date range:
    ///     - 200 OK: Returns a list of AuditLog items based on the specified date filter.
    ///     - 400 Bad Request: If an error occurs during the retrieval process, returns an error message.
    /// </returns>
    /// <remarks>
    ///     If fromDate and/or toDate are not provided, the method returns an unfiltered list of AuditLog items.
    /// </remarks>
    [HttpGet("Date")]
    [ProducesResponseType(typeof(List<AuditLog>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFiltered(string? fromDate, string? toDate)
    {
        try
        {
            return Ok(await _context.GetFiltered(fromDate, toDate));
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Authenticates a user by validating the provided credentials (email and password).
    /// </summary>
    /// <param name="name">name of user (optional).</param>
    /// <returns>
    ///     Returns an HTTP response indicating the result of the authentication attempt:
    ///     - 200 OK: Successful authentication.
    ///     - 400 Bad Request: Invalid email address format.
    ///     - 401 Unauthorized: Invalid email or password.
    ///     - 500 Internal Server Error: Unexpected errors during the authentication process.
    /// </returns>
    [HttpGet("Name")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SortByUserName(string name)
    {
        try
        {
            return Ok(await _context.SortByUserName(name));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
    } 
}