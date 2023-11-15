using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestProject.Services.Implementation;
using TestProject.Services.Interfaces;

namespace TestProject.Controllers;
[Authorize(Roles = "ADMIN")]
public class AuditViewController : Controller
{
    private readonly IAuditRepository _context;
    public AuditViewController(IAuditRepository context) => _context = context;
    public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, string Name) => View(await _context.Index(fromDate, toDate, Name));
}