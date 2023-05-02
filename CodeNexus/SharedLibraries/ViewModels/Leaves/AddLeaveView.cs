using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class AddLeaveView
    {
        public ApplyLeavesView LeaveDetails { get; set; }
        public List<DocumentsToUpload> ListOfDocuments { get; set; }
        public int ShiftId { get; set; }
        public int DepartmentId { get; set; }
        public int LocationId { get; set; }
    }
}
