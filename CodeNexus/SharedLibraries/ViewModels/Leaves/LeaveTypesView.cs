using System;

namespace SharedLibraries.ViewModels.Leaves
{
    public  class LeaveTypesView
    {
        public int? EmployeeId { get; set; }
        public int? LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public decimal? MandatoryDays { get; set; }
        public decimal? LeaveBalance { get; set; }
    }
    public class LeaveTypesDetailView
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveCode { get; set; }
        public int? LeaveAccruedType { get; set; }
        public string LeaveAccruedDay { get; set; }
        public decimal? LeaveAccruedNoOfDays { get; set; }
        public string LeaveDescription { get; set; }
        public bool? ProRate { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
    }
}