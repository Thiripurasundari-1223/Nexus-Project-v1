using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels.Appraisal;
using System.Collections.Generic;
using System.Linq;

namespace Appraisal.DAL.Repository
{
    public interface IAppraisalRepository : IBaseRepository<EntityMaster>
    {
        EntityMaster GetByID(int entityId);
        EntityMaster GetByName(string entityName);
        List<EntityMaster> GetAllEntityDetails();
        int GetEntityIdByName(string entityName);
        public bool EntityNameDuplication(string entityName,int entityId);
    }
    public class AppraisalRepository : BaseRepository<EntityMaster>, IAppraisalRepository
    {
        private readonly AppraisalDBContext dbContext;
        public AppraisalRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public EntityMaster GetByID(int entityId)
        {
            return dbContext.EntityMaster.Where(x => x.ENTITY_ID == entityId).FirstOrDefault();
        }
        public EntityMaster GetByName(string entityName)
        {
            return entityName==null?null:dbContext.EntityMaster.Where(x => x.ENTITY_NAME.ToLower() == entityName.ToLower()).FirstOrDefault();
        }
        public List<EntityMaster> GetAllEntityDetails()
        {
            return dbContext.EntityMaster.OrderByDescending(x=>x.CREATED_DATE).ToList();
        }
        public int GetEntityIdByName(string entityName)
        {
            return entityName==null?0:dbContext.EntityMaster.Where(x => x.ENTITY_NAME.ToLower() == entityName.ToLower()).Select(x => x.ENTITY_ID).FirstOrDefault();
        }
        public bool EntityNameDuplication(string entityName,int entityId)
        {
            bool isDuplicateName = false;
            string existEntityName = dbContext.EntityMaster.Where(x => x.ENTITY_NAME.ToLower() == entityName.ToLower() && (x.ENTITY_ID == entityId || entityId == 0)).Select(x => x.ENTITY_NAME).FirstOrDefault();
            if (entityId == 0 && existEntityName != null)
            {
                isDuplicateName = true;
            }
            else if(entityId != 0 && existEntityName?.ToLower() != entityName?.ToLower())
            {
                string newEntityName = dbContext.EntityMaster.Where(x => x.ENTITY_NAME.ToLower() == entityName.ToLower()).Select(x => x.ENTITY_NAME).FirstOrDefault();
                if(newEntityName != null)
                {
                    isDuplicateName = true;
                }
            }            
            return isDuplicateName;
        }
    }
}