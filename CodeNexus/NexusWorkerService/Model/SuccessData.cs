using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusWorkerService.Model
{
    public class SuccessData
    {
        public string StatusCode { get; set; }
        public string StatusText { get; set; }
        public dynamic Data { get; set; }
    }
}
