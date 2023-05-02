using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EducationDetailView
    {
		public int EducationDetailId { get; set; }
		public int? EmployeeId { get; set; }
		public string InstitutionName { get; set; }
		public string UniversityName { get; set; }
		public int? EducationTypeId { get; set; }
		public string EducationType { get; set; }
		public int? BoardId { get; set; }
		public string BoardName { get; set; }
		public DateTime? YearOfCompletion { get; set; }
		public DateTime? EndDate { get; set; }
		public decimal? MarkPercentage { get; set; }
        //public int? DegreeId { get; set; }
        //public int? SpecializationId { get; set; }
        public string Degree { get; set; }
        public string Specialization { get; set; }
        public int? ExpiryYear { get; set; }
		public string CertificateName { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
		public DocumentsToUpload Marksheet { get; set; }
		public DocumentsToUpload Certificate { get; set; }
		public List<DocumentsToUpload> DocumentDetails { get; set; }

	}
}
