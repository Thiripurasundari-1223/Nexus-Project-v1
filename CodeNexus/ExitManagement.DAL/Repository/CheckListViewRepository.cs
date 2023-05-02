using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{
    
    public interface ICheckListViewRepository : IBaseRepository<CheckListView>
    {
        CheckListView GetChecklistViewByID(int checklistId);
        List<string> GetCheckListViewByRole(List<string> roleList);
        List<CheckListView> GetAllChecklistView();
    }


    public class CheckListViewRepository : BaseRepository<CheckListView>, ICheckListViewRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public CheckListViewRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public CheckListView GetChecklistViewByID(int checklistId)
        {
            return dbContext.CheckListView.Where(x => x.CheckListViewId == checklistId).FirstOrDefault();
        }
        public List<string> GetCheckListViewByRole(List<string> roleList)
        {
            List<string> checkListView = new List<string>();
            List<CheckListView> listView= dbContext.CheckListView.Where(x => roleList.Contains(x.ApproverRole.ToLower())).ToList();
            foreach(var item in listView)
            {
                if(item.Manager==true)
                {
                    checkListView.Add("manager");
                }
                if (item.PMO == true)
                {
                    checkListView.Add("pmo");
                }
                if (item.IT == true)
                {
                    checkListView.Add("it");
                }
                if (item.Admin == true)
                {
                    checkListView.Add("admin");
                }
                if (item.Finance == true)
                {
                    checkListView.Add("finance");
                }
                if (item.HR == true)
                {
                    checkListView.Add("hr");
                }
            }
            return checkListView.Distinct().ToList();
        }
        public List<CheckListView> GetAllChecklistView()
        {
            return dbContext.CheckListView.ToList();
        }
    }
}
