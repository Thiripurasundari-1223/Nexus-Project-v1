using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class ResignationReason
    {
        [Key]
        public int ResignationReasonId { get; set; }
        public string ResignationReasonName { get; set; }
        public bool IsInvoluntary { get; set; }
    }
}
