using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class AbsentSettingsView
    {
        public int AbsentSettingId { get; set; }
        public bool? Gender_Male_Applicable { get; set; }
        public bool? Gender_Female_Applicable { get; set; }
        public bool? Gender_Others_Applicable { get; set; }
        public bool? MaritalStatus_Single_Applicable { get; set; }
        public bool? MaritalStatus_Married_Applicable { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public List<int> AbsentApplicableDepartmentId { get; set; }
        public List<int> AbsentExceptionDepartmentId { get; set; }
        public List<int> AbsentApplicableDesignationId { get; set; }
        public List<int> AbsentExceptionDesignationId { get; set; }
        public List<int> AbsentApplicableLocationId { get; set; }
        public List<int> AbsentExceptionLocationId { get; set; }
        public List<int> AbsentApplicableRoleId { get; set; }
        public List<int> AbsentExceptionRoleId { get; set; }
        public List<int> AbsentApplicableEmployeeTypeId { get; set; }
        public List<int> AbsentExceptionEmployeeTypeId { get; set; }
        public List<int> AbsentApplicableProbationStatusId { get; set; }
        public List<int> AbsentExceptionProbationStatusId { get; set; }
        public List<int> AbsentApplicableEmployeeId { get; set; }
        public List<int> AbsentExceptionEmployeeId { get; set; }
        //public AbsentExceptionView AbsentExceptionView { get; set; }
        public AbsentRestrictionView AbsentRestriction { get; set; }
        public bool? Gender_Male_Exception { get; set; }
        public bool? Gender_Female_Exception { get; set; }
        public bool? Gender_Others_Exception { get; set; }
        public bool? MaritalStatus_Single_Exception { get; set; }
        public bool? MaritalStatus_Married_Exception { get; set; }
    }
    public class AbsentExceptionView
    {
        public bool? Gender_Male_Exception { get; set; }
        public bool? Gender_Female_Exception { get; set; }
        public bool? Gender_Others_Exception { get; set; }
        public bool? MaritalStatus_Single_Exception { get; set; }
        public bool? MaritalStatus_Married_Exception { get; set; }
        public int? AbsentSettingId { get; set; }
    }
    public class AbsentRestrictionView
    {
        public bool? WeekendsBetweenAttendacePeriod { get; set; }
        public bool? HolidaysBetweenAttendancePeriod { get; set; }
        public int? AbsentSettingId { get; set; }
    }
}
