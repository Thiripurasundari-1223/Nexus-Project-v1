using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{

    public class DocumentDetail
    {
        public int DOC_ID { get; set; }
        public string DOC_NAME { get; set; }
        public string DOC_DESCRIPTION { get; set; }
        public string DOC_TYPE { get; set; }
        public int? APP_CYCLE_ID { get; set; }
        public int? OBJECTIVE_ID { get; set; }
        public int? KEY_RESULT_ID { get; set; }

    }
    public class EmployeeKeyResultAttachmentsView
    {
        public List<DocumentDetail> ListOfDocuments { get; set; }
        public int APP_CYCLE_ID { get; set; }
        public int EMPLOYEE_ID { get; set; }
        public int OBJECTIVE_ID { get; set; }
        public int KEY_RESULT_ID { get; set; }
        public int DOC_UPLOADED_BY { get; set; }
        public string SourceType { get; set; }
        public int? CREATED_BY { get; set; }
        public string BaseDirectory { get; set; }
        public string APP_CYCLE_NAME { get; set; }
        public string OBJECTIVE_NAME { get; set; }


    }



}
