using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class ManagerCheckList
    {
	public int ManagerCheckListId{ get; set; } 
    public int? ResignationChecklistId{ get; set; } 
	public int? ApprovedBy { get; set; } 
	public string Status{ get; set; } 
    public string KnowledgeTransferId { get; set; } 
	public string KnowledgeTransferRemark { get; set; }
	public string MailID { get; set; } 
	public string RoutedTo { get; set; }
	public string RoutedToRemark { get; set; } 
	public string ProjectDocumentsReturnedId{ get; set; } 
	public string ProjectDocumentsReturnedRemark { get; set; }
	public string RecoverPayNoticeId { get; set; }
	public string RecoverPayNoticeRemark { get; set; }
	public string RouteReporteesTo{ get; set; } 
	public string WaivingOffNoticePeriodReason { get; set; }
	public string TimesheetsId { get; set; }
	public string TimesheetsRemark { get; set; }
	public int? CreatedBy{ get; set; }
	public DateTime? CreatedOn{ get; set; } 
	public int? ModifiedBy{ get; set; } 
	public DateTime? ModifiedOn { get; set; }
	}
}
