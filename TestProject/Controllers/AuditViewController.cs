using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.ExtensionFunctions;
using TestProject.ViewModels;

namespace TestProject.Controllers;
[Authorize(Roles = "ADMIN")]
public class AuditViewController : Controller
{
    private readonly AppDbContext _context;
    public AuditViewController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, string Name)
    {
        var auditLogs = await _context.AuditLog.ToListAsync();

        var filteredLogs = ForAudit.FilterAuditLogsByDate(auditLogs, fromDate, toDate,Name);

        var viewModel = new AuditLogViewModel
        {
            FromDate = fromDate ?? DateTime.Today.AddDays(-100), 
            ToDate = toDate ?? DateTime.Today,
            FilteredLogs = filteredLogs
        };

        return View(viewModel);
    }
}