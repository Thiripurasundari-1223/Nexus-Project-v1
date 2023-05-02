using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
    public class EducationDetail
    {
		[Key]
		public int EducationDetailId { get; set; }
		public int? EmployeeId { get; set; }
		public string InstitutionName { get; set; }
		public string UniversityName { get; set; }
		public int? EducationTypeId { get; set; }
		public int? BoardId { get; set; }
		public DateTime? YearOfCompletion { get; set; }
		//public int? DegreeId { get; set; }
		//public int? SpecializationId { get; set; }
		public string Degree { get; set; }
		public string Specialization { get; set; }
		public int? ExpiryYear { get; set; }
		public decimal? MarkPercentage { get; set; }
		public string CertificateName { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
    }
}
