﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TestProject.Data;
using TestProject.Domains;
using TestProject.ViewModels;

namespace TestProject.Controllers;
[Authorize(Roles = "ADMIN")]
public class AuditViewController : Controller
{
    private readonly AppDbContext _context;

    public AuditViewController(AppDbContext context) => _context = context;

    //public async Task<IActionResult> Index()
    //{
    //    var listAudit = await _context.AuditLog.ToListAsync();
    //    return View("Index", listAudit);
    //}


    // GET: /AuditLogs
    public async Task<IActionResult>Index(DateTime? fromDate, DateTime? toDate, string Name)
    {
        var auditLogs = await _context.AuditLog.ToListAsync();

        var filteredLogs = FilterAuditLogsByDate(auditLogs, fromDate, toDate,Name);

        // Create the view model and set the filtered logs
        var viewModel = new AuditLogViewModel
        {
            FromDate = fromDate ?? DateTime.Today.AddDays(-100), 
            ToDate = toDate ?? DateTime.Today,
            FilteredLogs = filteredLogs
        };



        return View(viewModel);
    }



    private static List<AuditLog> FilterAuditLogsByDate(List<AuditLog> logs, DateTime? fromDate, DateTime? toDate ,string Name)
    {
        var filteredLogs = logs
               .Where(log =>
                   (!fromDate.HasValue || log.DateTime >= fromDate) &&
                   (!toDate.HasValue || log.DateTime <= toDate?.AddDays(1))&&
                   (Name == null || log.UserName.IndexOf(Name, StringComparison.OrdinalIgnoreCase) >= 0)
               )
               .ToList();
        return filteredLogs;
    }
}