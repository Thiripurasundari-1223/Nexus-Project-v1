using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class ExitManagementEmailTemplate
    {
        [Key]
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string Subject { get; set; }

        public string Body { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

    }
}
