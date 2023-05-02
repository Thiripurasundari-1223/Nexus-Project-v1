using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels.Reports
{
    public class ReportsModel
    {
        [Key]
        public int Id { get; set; }
        public string ReportData { get; set; }
        public string ReportDetail { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }    
}