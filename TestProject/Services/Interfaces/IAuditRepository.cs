using TestProject.Domains;
using TestProject.ViewModels;

namespace TestProject.Services.Interfaces;

public interface IAuditRepository
{
    Task<AuditLogViewModel> Index(DateTime? fromDate, DateTime? toDate, string Name);
    Task<List<AuditLog>> SortByUserName(string name);
    Task<List<AuditLog>> GetFiltered(string? fromDate, string? toDate);
}