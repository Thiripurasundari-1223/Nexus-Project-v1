using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Appraisal
{
    public class WorkdayObjective
    {
        [Key]
        public int WorkdayObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public int WorkDayId { get; set; }

    }
}
