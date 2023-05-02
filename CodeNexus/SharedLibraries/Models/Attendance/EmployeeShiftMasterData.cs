using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Attendance
{
    public class EmployeeShiftMasterData
    {
        [Key]
        public int ShiftDetailsId { get; set; }
        public string ShiftName { get; set; }
    }
}
