using SharedLibraries.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Timesheet
{
    public class TeamMember
    {
        public List<TeamMemberDetails> ListOfTeamMember { get; set; }
        public int ResourceId { get; set; }
        public DateTime? WeekStartDate { get; set; }
    }
}
