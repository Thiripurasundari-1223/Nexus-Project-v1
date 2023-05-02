using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels
{
    public class ApproveOrRejectChangeRequest
    {
        public int ChangeRequestId { get; set; }
        public int ProjectId { get; set; }
        public string ChangeRequestStatus { get; set; }
        public string Comments { get; set; }
        public int EngineeringLeadId { get; set; }
        public int FinanceManagerId { get; set; }
    }
}