using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{
    
    public interface IExitManagementEmailTemplateRepository : IBaseRepository<ExitManagementEmailTemplate>
    {
        ExitManagementEmailTemplate GetEmailTemplateByName(string templateName);
        List<ExitManagementEmailTemplate> GetAllEmailTemplate();
    }


    public class ExitManagementEmailTemplateRepository : BaseRepository<ExitManagementEmailTemplate>, IExitManagementEmailTemplateRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public ExitManagementEmailTemplateRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public ExitManagementEmailTemplate GetEmailTemplateByName(string templateName)
        {
            return dbContext.ExitManagementEmailTemplate.Where(x => x.TemplateName == templateName).FirstOrDefault();

        }
        public List<ExitManagementEmailTemplate> GetAllEmailTemplate()
        {
            return dbContext.ExitManagementEmailTemplate.ToList();

        }
    }
}
