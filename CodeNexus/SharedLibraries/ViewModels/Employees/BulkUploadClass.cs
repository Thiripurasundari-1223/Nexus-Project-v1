using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class BulkUploadClass
    {
        public string DocumentName { get; set; }
        public decimal DocumentSize { get; set; }
        public string DocumentAsBase64 { get; set; }
        public int UploadedBy { get; set; }
    }
}
