using System;
namespace TestProject.Domains;

public class AuditLog
{
    public int Id { get; set; }
    public string? Action { get; set; }
    public string? UserName { get; set; }
    public string? ControllerName { get; set; }
    public DateTime? DateTime { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }

}