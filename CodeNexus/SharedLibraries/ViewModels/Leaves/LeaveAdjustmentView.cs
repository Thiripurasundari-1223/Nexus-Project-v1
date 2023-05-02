using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveAdjustmentView
    {
        public List<EmployeeLeaveAdjustmentView> LeaveAdjustments { get; set; }
        public LeaveBalanceView LeaveBalanceView { get; set; }
        public LeavesMasterDataView LeavesMasterData { get; set; }
        //public List<Department> EmployeeDepartmentList { get; set; }
        //public List<Designation> EmployeeDesignationList { get; set; }
    }

    public class ReporteesView
    {
        public int Employeeid { get; set; }
        public bool? isListView { get; set; }
        public bool? isManager { get; set; }
    }

}
