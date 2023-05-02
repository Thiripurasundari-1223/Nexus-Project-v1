using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class IndividualAppraisalCommentsView
    {
        public int App_Cycle_Id { get; set; }
        public int? Employee_Id { get; set; }
        public int CommentId{ get; set; }
        public string Comment { get; set; }
        public DateTime Comment_DateTime { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedBy_Name { get; set; }

    }
}
