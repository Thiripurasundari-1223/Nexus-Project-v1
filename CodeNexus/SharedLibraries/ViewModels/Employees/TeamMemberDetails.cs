namespace SharedLibraries.ViewModels
{
    public class TeamMemberDetails
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public decimal Allocation { get; set; }
        public int ReportingPersonId { get; set; }
        public string UserRole { get; set; }
        public string FormattedEmployeeId { get; set; }
        public int? ShiftId { get; set; }
    }
}