using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class EmployeeShiftDetailView
    {
        public HalfDay HalfDay { get; set; }
        public HalfDay FullDay { get; set; }
        public HalfDay Absent { get; set; }
        public bool? IsActive { get; set; }
    }
    public class HalfDay
    {
        public string OperatorOne { get; set; }
        public string Operatortwo { get; set; }
        public string FromValue { get; set; }
        public string ToValue { get; set; }
    }
}
