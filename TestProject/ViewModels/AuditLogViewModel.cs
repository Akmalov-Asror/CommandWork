using TestProject.Domains;

namespace TestProject.ViewModels
{
    public class AuditLogViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<AuditLog> FilteredLogs { get; set; }
    }
}
