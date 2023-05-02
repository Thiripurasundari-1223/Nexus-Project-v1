using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class EmployeeKRCommentView
    {
        public List<KraComments> KraComments { get; set; }
        public List<DocumentDetail> ListOfDocuments { get; set; }
    }
    public class KraComments
    {
        public int App_Cycle_Id { get; set; }
        public int Objective_Id { get; set; }
        public int Key_Result_Id { get; set; }
        public int Comment_Id { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDateTime { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }

    }
}