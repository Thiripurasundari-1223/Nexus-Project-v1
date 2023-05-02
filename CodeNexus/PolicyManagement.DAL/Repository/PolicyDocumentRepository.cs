using PolicyManagement.DAL.DBContext;
using PolicyManagement.DAL.Models;
using SharedLibraries.ViewModels.PolicyManagement;

namespace PolicyManagement.DAL.Repository
{
    public interface IPolicyDocumentRepository : IBaseRepository<PolicyDocuments>
    {
        List<PolicyDocuments>? GetPolicyDocument(int UserId, int LocationId, int RoleId, int DepartmentId, string DocType,
                                                    int CurrentWorkLocationId, int CurrentWorkPlaceId);
        int? GetPolicyAcknowledgementByEmployee(int UserId, int LocationId, int RoleId, int DepartmentId,
                                                    int CurrentWorkLocationId, int CurrentWorkPlaceId);
        void UpdatePolicyAcknowledgement(PolicyAcknowledgementView document);
    }

    public class PolicyDocumentRepository : BaseRepository<PolicyDocuments>, IPolicyDocumentRepository
    {
        private readonly PolicyMgmtDBContext _dbContext;
        public PolicyDocumentRepository(PolicyMgmtDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public List<PolicyDocuments>? GetPolicyDocument(int UserId, int LocationId, int RoleId, int DepartmentId, string DocType,
                                                            int CurrentWorkLocationId, int CurrentWorkPlaceId)
        {
            List<PolicyDocuments>? policyDocuments = null;
            if (UserId > 0 && DocType.ToLower() == "employee")
            {
                policyDocuments = (from pd in _dbContext.PolicyDocuments
                                   where (pd.EmployeeId == UserId && pd.ValidTo == null)
                                   select pd).ToList();
            }
            else
            {
                policyDocuments = (from pd in _dbContext.PolicyDocuments
                                   join sd in _dbContext.SharePolicyDocuments on pd.PolicyDocumentId equals sd.PolicyDocumentId into sp
                                   from spd in sp.DefaultIfEmpty()
                                   where pd.ValidTo == null && (pd.ToShareWithAll == true || spd.RoleId == RoleId ||
                                   spd.DepartmentId == DepartmentId || spd.LocationId == LocationId)
                                   select pd).ToList();
            }
            return policyDocuments;
        }
        public int? GetPolicyAcknowledgementByEmployee(int UserId, int LocationId, int RoleId, int DepartmentId,
                                                            int CurrentWorkLocationId, int CurrentWorkPlaceId)
        {
            List<PolicyDocuments>? policyDocuments = null;
            policyDocuments = (from pd in _dbContext.PolicyDocuments
                               join sd in _dbContext.SharePolicyDocuments on pd.PolicyDocumentId equals sd.PolicyDocumentId into sp
                               from spd in sp.DefaultIfEmpty()
                               where pd.Acknowledgement == true && pd.ValidTo == null && (pd.ToShareWithAll == true ||
                               spd.RoleId == RoleId || spd.DepartmentId == DepartmentId || spd.LocationId == LocationId)
                               select pd).OrderByDescending(x => x.CreatedOn).ToList();
            foreach (var policyDocument in policyDocuments)
            {
                PolicyAcknowledged? policyAcknowledged = _dbContext.PolicyAcknowledged.
                    Where(x => x.PolicyDocumentId == policyDocument.PolicyDocumentId).FirstOrDefault();
                if (policyAcknowledged != null && policyAcknowledged.AcknowledgedStatus != null &&
                    policyAcknowledged.AcknowledgedStatus.ToString().ToLower() != "agreed")
                {
                    return policyDocument.PolicyDocumentId;
                }
                if (policyAcknowledged == null)
                {
                    return policyDocument.PolicyDocumentId;
                }
            }
            return 0;
        }

        public void UpdatePolicyAcknowledgement(PolicyAcknowledgementView document)
        {
            PolicyAcknowledged policyAcknowledged = new()
            {
                EmployeeId = document.AcknowledgedBy,
                PolicyDocumentId = document.PolicyDocumentId,
                AcknowledgedAt = DateTime.UtcNow,
                AcknowledgedStatus = document.AcknowledgedStatus
            };
            _dbContext.Add(policyAcknowledged);
            _dbContext.SaveChanges();
        }
    }

    public interface ISharePolicyDocumentRepository : IBaseRepository<SharePolicyDocuments>
    {
        List<SharePolicyDocuments>? GetSharePolicyDocumentsByPolicyDocumentId(int PolicyDocumentId);
    }

    public class SharePolicyDocumentRepository : BaseRepository<SharePolicyDocuments>, ISharePolicyDocumentRepository
    {
        private readonly PolicyMgmtDBContext _dbContext;
        public SharePolicyDocumentRepository(PolicyMgmtDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public List<SharePolicyDocuments>? GetSharePolicyDocumentsByPolicyDocumentId(int PolicyDocumentId)
        {
            return _dbContext.SharePolicyDocuments.Where(x => x.PolicyDocumentId == PolicyDocumentId).ToList();
        }
    }

    public interface IFolderRepository : IBaseRepository<Folder>
    {
        List<FolderView> GetAllFolders();
    }

    public class FolderRepository : BaseRepository<Folder>, IFolderRepository
    {
        private readonly PolicyMgmtDBContext _dbContext;
        public FolderRepository(PolicyMgmtDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<FolderView> GetAllFolders()
        {
            return _dbContext.Folders.Select(x => new FolderView
            {
                FolderId = x.FolderId,
                FolderName = x.FolderName
            }).ToList();
        }
    }

    public interface IPolicyAcknowledgedRepository : IBaseRepository<PolicyAcknowledged>
    {
    }

    public class PolicyAcknowledgedRepository : BaseRepository<PolicyAcknowledged>, IPolicyAcknowledgedRepository
    {
        public PolicyAcknowledgedRepository(PolicyMgmtDBContext dbContext) : base(dbContext)
        {
        }
    }
}