using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class ITCheckList
    {
        public int ITCheckListId { get; set; }
        public int? ResignationChecklistId { get; set; }
        public int? ApprovedBy { get; set; }
        public string Status { get; set; }
        public string LoginDisabledId { get; set; }
        public string LoginDisabledRemark { get; set; }
        public string MailID { get; set; }
        public string RoutedToRemark { get; set; }
        public string BiometricAccessTerminationId { get; set; }
        public string BiometricAccessTerminationRemark { get; set; }
        public string SystemAssetsRecoveredId { get; set; }
        public string SystemAssetsRecoveredRemark { get; set; }
        public string DATAcardReturnedId { get; set; }
        public string DATAcardReturnedRemark { get; set; }
        public string DamageRecoveryId { get; set; }
        public string DamageRecoveryRemark { get; set; }
        public string MacAddressRemovalId { get; set; }
        public string MacAddressRemovalRemark { get; set; }
        public string DataBackUpId { get; set; }
        public string DataBackUpRemark { get; set; }
        public string UserLaptopDataSize { get; set; }
        public string UserLaptopDataSizeRemark { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
