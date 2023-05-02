using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Attendance
{
    public class AbsentSetting
    {
        [Key]
        public int AbsentSettingId { get; set; }
        public bool? Gender_Male_Applicable { get; set; }
        public bool? Gender_Female_Applicable { get; set; }
        public bool? Gender_Others_Applicable { get; set; }
        public bool? MaritalStatus_Single_Applicable { get; set; }
        public bool? MaritalStatus_Married_Applicable { get; set; }
        public bool? Gender_Male_Exception { get; set; }
        public bool? Gender_Female_Exception { get; set; }
        public bool? Gender_Others_Exception { get; set; }
        public bool? MaritalStatus_Single_Exception { get; set; }
        public bool? MaritalStatus_Married_Exception { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
