using SharedLibraries.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    public class HolidayEmployeeMasterData
    {
        public List<KeyWithValue> DepartmentList { get; set; }
        public List<KeyWithValue> ShiftList { get; set; }
        public List<KeyWithValue> LocationList { get; set; }
    }
}
