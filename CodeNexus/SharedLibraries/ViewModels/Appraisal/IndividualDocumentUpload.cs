using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
   public class IndividualDocumentUpload
    {
        public int DOC_ID { get; set; }
        public string DOC_NAME { get; set; }
        public string DOC_DESCRIPTION { get; set; }
        public string DOC_TYPE { get; set; }
        public string DocumentAsBase64 { get; set; }
        public byte[] DocumentAsByteArray { get; set; }
    }
}
