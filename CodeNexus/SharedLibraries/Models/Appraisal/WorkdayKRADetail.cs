using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Appraisal
{
    public class WorkdayKRA
    {
        [Key]
        public int WorkdayKRAId { get; set; }
        public string KRAName { get; set; }
        public int WorkdayObjectiveId { get; set; }
        public int WorkDayId { get; set; }
    }
}