using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;

namespace TestProject.Controllers;
[Authorize(Roles = "ADMIN")]
public class AuditViewController : Controller
{
    private readonly AppDbContext _context;

    public AuditViewController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var listAudit = await _context.AuditLog.ToListAsync();
        return View("Index", listAudit);
    }
}