using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface ISkillsetHistoryRepository : IBaseRepository<SkillsetHistory>
    {
        SkillsetHistoryView GetSkillsetHistoryBySkillsetId(int skillsetId);
    }
    public class SkillsetHistoryRepository : BaseRepository<SkillsetHistory>, ISkillsetHistoryRepository
    {
        private readonly IAMDBContext dbContext;
        public SkillsetHistoryRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public SkillsetHistoryView GetSkillsetHistoryBySkillsetId(int skillsetId)
        {
            SkillsetHistoryView skillsetHistory = new SkillsetHistoryView();
            var skillDetail = dbContext.Skillset.Where(x => x.SkillsetId == skillsetId).Select(x => x).FirstOrDefault();
            skillsetHistory.SkillName = skillDetail.Skillset;
            skillsetHistory.CreatedBy = dbContext.Employees.Where(y => y.EmployeeID == skillDetail.CreatedBy).Select(z => z.EmployeeName).FirstOrDefault();
            skillsetHistory.ModifiedBy = dbContext.Employees.Where(y => y.EmployeeID == skillDetail.ModifiedBy).Select(z => z.EmployeeName).FirstOrDefault();
            skillsetHistory.CreatedOn = skillDetail.CreatedOn;
            skillsetHistory.ModifiedOn = skillDetail.ModifiedOn;
            List<SkillsetHistoryListView> skillsetHistories = dbContext.SkillsetHistory.Where(x => x.SkillsetId == skillsetId).ToList().GroupBy(y => y.CreatedOn.Value.Date).Select(z =>
                new SkillsetHistoryListView
                {
                    historyDate = z.Key.Date,
                    skillsetHistory = z.Select(x =>
                      new SkillsetHistoryDetails
                      {
                          SkillsetHistoryId = x.SkillsetHistoryId,
                          SkillsetId = x.SkillsetId,
                          OldValue = x.OldValue,
                          NewValue = x.NewValue,
                          CreatedBy = dbContext.Employees.Where(y => y.EmployeeID == x.CreatedBy).Select(z => z.EmployeeName).FirstOrDefault(),
                          Category = x.Category,
                          CreatedOn = x.CreatedOn

                      }).OrderByDescending(x=>x.CreatedOn).ToList()
                }
            ).OrderByDescending(x=>x.historyDate).ToList();            
            skillsetHistory.skillsetHistoryList = skillsetHistories;
            return skillsetHistory;
        }

    }
}
