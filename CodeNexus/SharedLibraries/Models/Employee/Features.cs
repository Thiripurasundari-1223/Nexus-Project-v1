using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Employee
{
    public class Features
    {
		[Key]
		public int FeatureId { get; set; }
		public string FeatureName { get; set; }
		public string FeatureDescription { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
		public string NavigationURL { get; set; }
	}
}