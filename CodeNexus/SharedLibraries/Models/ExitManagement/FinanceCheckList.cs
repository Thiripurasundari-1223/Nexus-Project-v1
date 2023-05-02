using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
	public class FinanceCheckList
	{
		public int FinanceCheckListId { get; set; }
		public int? ResignationChecklistId { get; set; }
		public int? ApprovedBy { get; set; }
		public string Status { get; set; }
		public string JoiningBonus { get; set; }
		public string JoiningBonusRemark { get; set; }
		public string RetentionBonus { get; set; }
		public string RetentionBonusRemark { get; set; }
		public string SalaryAdvance { get; set; }
		public string SalaryAdvanceRemark { get; set; }
		public string Loans { get; set; }
		public string LoansRemark { get; set; }
		public string TravelAdvance { get; set; }
		public string TravelAdvanceRemark { get; set; }
		public string TravelCardReturned { get; set; }
		public string TravelCardReturnedRemark { get; set; }
		public string RelocationCost { get; set; }
		public string RelocationCostRemark { get; set; }
		public string TravelKitAllowance { get; set; }
		public string TravelKitAllowanceRemark { get; set; }
		public string ITProofsId { get; set; }
		public string ITProofsRemark { get; set; }
		public string TrainingBond { get; set; }
		public string TrainingBondRemark { get; set; }
		public string ITRecovery { get; set; }
		public string ITRecoveryRemark { get; set; }
		public string AdministrationRecovery { get; set; }
		public string AdministrationRecoveryRemark { get; set; }
		public string GratuityEligibilityId { get; set; }
		public string GratuityEligibilityRemark { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? ModifiedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
	}
}
