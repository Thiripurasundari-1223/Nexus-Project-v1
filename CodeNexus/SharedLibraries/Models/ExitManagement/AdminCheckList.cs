using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class AdminCheckList
    {
        public int AdminCheckListId { get; set; }
        public int? ResignationChecklistId { get; set; }
        public int? ApprovedBy { get; set; }
        public string Status { get; set; }
        public string IdentityCardId { get; set; }
        public string IdentityCardRemark { get; set; }
        public string CabinKeysID { get; set; }
        public string CabinKeysRemark { get; set; }
        public string TravelCardId { get; set; }
        public string TravelCardRemark { get; set; }
        public string BusinessCardsId { get; set; }
        public string BusinessCardsRemark { get; set; }
        public string LibraryBooksId { get; set; }
        public string LibraryBooksRemark { get; set; }
        public string CompanyMobileId { get; set; }
        public string CompanyMobileRemark { get; set; }
        public string OtherRecovery { get; set; }
        public string BiometricAccessTerminationId { get; set; }
        public string BiometricAccessTerminationRemark { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
