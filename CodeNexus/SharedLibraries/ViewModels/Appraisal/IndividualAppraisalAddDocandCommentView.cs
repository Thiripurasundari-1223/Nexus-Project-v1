using System;
using System.Collections.Generic;
using System.Text;
using SharedLibraries.Models.Appraisal;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class IndividualAppraisalAddDocandCommentView
    {
        
        public IndividualComments IndividualComments { get; set; }
        public IndividualDocDetails IndividualDocDetails { get; set; }
        public List<IndividualDocumentUpload> ListOfDocuments { get; set; }
    }

    public class IndividualDocDetails
    {
        public int AppCycleID { get; set; }
        public int ObjectiveID { get; set; }
        public int KeyResultID { get; set; }
        public int EmployeeID { get; set; }
        public int? CreatedBy { get; set; }
        public string AppCycle_Name { get; set; }
        public string Objective_Name { get; set; }
    }
    public class IndividualComments 
    {
        public int CommentId { get; set; }
        public int AppCycleId { get; set; }
        public int EmployeeId { get; set; }
        public int ObjectiveId { get; set; }
        public int KeyResultId { get; set; }
        public string Comment { get; set; }
        public int? Created_By { get; set; }
    }
    
    
}

