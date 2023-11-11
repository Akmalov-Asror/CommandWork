using System;
namespace TestProject.Domains
{
	public class AuditLog
	{
		public int Id { get; set; }
		public string? Action { get; set; }
		public string? UserName { get; set; }
		public string? ControllerName { get; set; }
		public DateTime? DateTime { get; set; }
        public int? OldValueId { get; set; }
        public int? NewValueId { get; set; }
        public Product? OldValue { get; set; }
		public Product? NewValue { get; set; }

	}
}