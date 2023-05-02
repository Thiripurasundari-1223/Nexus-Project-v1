using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveGrantDocumentUpload
    {
        public int DOC_ID { get; set; }
        public string DOC_NAME { get; set; }
        public string DOC_TYPE { get; set; }
        public string DocumentAsBase64 { get; set; }
        public byte[] DocumentAsByteArray { get; set; }
    }
}
