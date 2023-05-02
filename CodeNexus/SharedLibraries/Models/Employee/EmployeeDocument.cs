using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
	public class EmployeeDocument
	{
		[Key]
	    public int? EmployeeDocumentId { get; set; }
		public int? EmployeeID {get; set;}
		public int? SourceId { get; set; }
		public string SourceType { get; set; }
		public string DocumentType { get; set; }
		public string DocumentName {get;set;}
		public string DocumentPath {get;set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int CreatedBy { get; set; }
		public int ModifiedBy { get; set; }
	}
}
