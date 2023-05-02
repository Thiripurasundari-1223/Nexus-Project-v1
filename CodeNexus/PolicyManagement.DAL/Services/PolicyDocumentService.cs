using PolicyManagement.DAL.Models;
using PolicyManagement.DAL.Repository;
using SharedLibraries.ViewModels.PolicyManagement;

namespace PolicyManagement.DAL.Services
{
    public class PolicyDocumentService
    {
        private readonly IPolicyDocumentRepository _policyDocumentRepository;
        private readonly ISharePolicyDocumentRepository _sharePolicyDocumentRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IPolicyAcknowledgedRepository _policyAcknowledgedRepository;

        public PolicyDocumentService(IFolderRepository folderRepository, IPolicyDocumentRepository policyDocumentRepository,
            ISharePolicyDocumentRepository sharePolicyDocumentRepository, IPolicyAcknowledgedRepository policyAcknowledgedRepository)
        {
            _folderRepository = folderRepository;
            _policyDocumentRepository = policyDocumentRepository;
            _sharePolicyDocumentRepository = sharePolicyDocumentRepository;
            _policyAcknowledgedRepository = policyAcknowledgedRepository;
        }

        public List<FolderView> GetFolders()
        {
            return _folderRepository.GetAllFolders();
        }

        public PolicyDocumentView? GetPolicyDocumentById(int PolicyDocumentId, int UserId)
        {
            return GetPolicyDocument(PolicyDocumentId: PolicyDocumentId, UserId: UserId)?.FirstOrDefault();
        }

        public List<PolicyDocumentView>? GetPolicyDocumentByUserId(int UserId = 0, string DocType = "", int LocationId = 0, int RoleId = 0,
            int DepartmentId = 0, int CurrentWorkLocationId = 0, int CurrentWorkPlaceId = 0)
        {
            return GetPolicyDocument(UserId: UserId, DocType: DocType, LocationId: LocationId, RoleId: RoleId, DepartmentId: DepartmentId,
                CurrentWorkLocationId: CurrentWorkLocationId, CurrentWorkPlaceId: CurrentWorkPlaceId);
        }

        private List<PolicyDocumentView>? GetPolicyDocument(int UserId = 0, int PolicyDocumentId = 0, string DocType = "",
            int LocationId = 0, int RoleId = 0, int DepartmentId = 0, int CurrentWorkLocationId = 0, int CurrentWorkPlaceId = 0)
        {
            List<PolicyDocumentView>? policyDocumentViews = new();
            List<PolicyDocuments>? policyDocuments;
            if (UserId == 0 && PolicyDocumentId == 0)
            {
                if (DocType.ToLower() == "policy")
                    policyDocuments = _policyDocumentRepository.GetAll().Where(x => x.EmployeeId == null && x.ValidTo == null).ToList();
                else
                    policyDocuments = _policyDocumentRepository.GetAll().Where(x => x.EmployeeId != null && x.ValidTo == null).ToList();
            }
            else if (PolicyDocumentId > 0)
                policyDocuments = _policyDocumentRepository.GetAll().Where(x => x.PolicyDocumentId == PolicyDocumentId).ToList();
            else
                policyDocuments = _policyDocumentRepository.GetPolicyDocument(UserId, LocationId, RoleId, DepartmentId,
                                                                                DocType, CurrentWorkLocationId, CurrentWorkPlaceId);
            if (policyDocuments?.Count > 0)
            {
                policyDocuments = policyDocuments.OrderByDescending(x => x.CreatedOn).ToList();
                foreach (PolicyDocuments policyDocument in policyDocuments)
                {
                    PolicyDocumentView? policyDocumentView;
                    if (policyDocument != null)
                    {
                        string[] NotifyThrough = Array.Empty<string>();
                        if (policyDocument.IsNotifyViaEmail == true && policyDocument.IsNotifyViaFeeds == true)
                        {
                            NotifyThrough = new string[2];
                            NotifyThrough[0] = "email";
                            NotifyThrough[1] = "notification";
                        }
                        if (policyDocument.IsNotifyViaEmail == true && policyDocument.IsNotifyViaFeeds != true)
                        {
                            NotifyThrough = new string[1];
                            NotifyThrough[0] = "email";
                        }
                        if (policyDocument.IsNotifyViaEmail != true && policyDocument.IsNotifyViaFeeds == true)
                        {
                            NotifyThrough = new string[1];
                            NotifyThrough[0] = "notification";
                        }
                        string[] ToView = Array.Empty<string>();
                        if (policyDocument.IsEmployeeAbleToView == true && policyDocument.IsRMAbleToView == true)
                        {
                            ToView = new string[2];
                            ToView[0] = "employee";
                            ToView[1] = "reportingManager";
                        }
                        if (policyDocument.IsEmployeeAbleToView == true && policyDocument.IsRMAbleToView != true)
                        {
                            ToView = new string[1];
                            ToView[0] = "employee";
                        }
                        if (policyDocument.IsEmployeeAbleToView != true && policyDocument.IsRMAbleToView == true)
                        {
                            ToView = new string[1];
                            ToView[0] = "reportingManager";
                        }
                        string[] ToDownload = Array.Empty<string>();
                        if (policyDocument.IsEmployeeAbleToDownload == true && policyDocument.IsRMAbleToDownload == true)
                        {
                            ToDownload = new string[2];
                            ToDownload[0] = "employee";
                            ToDownload[1] = "reportingManager";
                        }
                        if (policyDocument.IsEmployeeAbleToDownload == true && policyDocument.IsRMAbleToDownload != true)
                        {
                            ToDownload = new string[1];
                            ToDownload[0] = "employee";
                        }
                        if (policyDocument.IsEmployeeAbleToDownload != true && policyDocument.IsRMAbleToDownload == true)
                        {
                            ToDownload = new string[1];
                            ToDownload[0] = "reportingManager";
                        }
                        policyDocumentView = new()
                        {
                            PolicyDocumentId = policyDocument.PolicyDocumentId,
                            FileName = policyDocument.FileName,
                            Description = policyDocument.Description,
                            FolderId = policyDocument.FolderId,
                            FolderName = policyDocument.FolderId > 0 ? _folderRepository.Get((int)policyDocument.FolderId).FolderName! : null,
                            ValidTo = policyDocument.ValidTo,
                            Acknowledgement = policyDocument.Acknowledgement,
                            AlreadyAcknowledged = GetPolicyAcknowledgement(policyDocument.PolicyDocumentId, UserId > 0 ? UserId : policyDocument.EmployeeId),
                            ToView = ToView,
                            ToDownload = ToDownload,
                            NotifyThrough = NotifyThrough,
                            CreatedOn = policyDocument.CreatedOn,
                            CreatedBy = policyDocument.CreatedBy,
                            FilePath = policyDocument.FilePath,
                            ModifiedBy = policyDocument.ModifiedBy,
                            ModifiedOn = policyDocument.ModifiedOn,
                            EmployeeId = policyDocument.EmployeeId,
                            ToShareWithAll = policyDocument.ToShareWithAll
                        };
                        if (!(policyDocument.ToShareWithAll == true || policyDocument.EmployeeId != null))
                        {
                            List<SharePolicyDocuments>? sharePolicyDocuments = _sharePolicyDocumentRepository.GetSharePolicyDocumentsByPolicyDocumentId(policyDocument.PolicyDocumentId);
                            if (sharePolicyDocuments?.Count > 0)
                            {
                                policyDocumentView.SharePolicyDocuments = new();
                                foreach (SharePolicyDocuments document in sharePolicyDocuments)
                                {
                                    SharePolicyDocumentView sharePolicyDocument = new()
                                    {
                                        PolicyDocumentId = document.PolicyDocumentId,
                                        DepartmentId = sharePolicyDocuments.Where(x => x.PolicyDocumentId == document.PolicyDocumentId).Select(y => y.DepartmentId)?.DistinctBy(z => z != null ? z.Value : 0).ToList() ?? null,
                                        LocationId = sharePolicyDocuments.Where(x => x.PolicyDocumentId == document.PolicyDocumentId).Select(y => y.LocationId)?.DistinctBy(z => z != null ? z.Value : 0).ToList() ?? null,
                                        RoleId = sharePolicyDocuments.Where(x => x.PolicyDocumentId == document.PolicyDocumentId).Select(y => y.RoleId)?.DistinctBy(z => z != null ? z.Value : 0).ToList() ?? null,
                                        CreatedOn = document.CreatedOn,
                                        CreatedBy = document.CreatedBy,
                                        ModifiedOn = document.ModifiedOn,
                                        ModifiedBy = document.ModifiedBy,
                                        SharePolicyDocumentId = document.SharePolicyDocumentId
                                    };
                                    sharePolicyDocument.DepartmentId?.RemoveAll(x => x == null);
                                    sharePolicyDocument.LocationId?.RemoveAll(x => x == null);
                                    sharePolicyDocument.RoleId?.RemoveAll(x => x == null);
                                    bool isAlreadyNot = true;
                                    foreach (SharePolicyDocumentView documentView in policyDocumentView.SharePolicyDocuments)
                                    {
                                        if (documentView.PolicyDocumentId == sharePolicyDocument.PolicyDocumentId) { isAlreadyNot = false; break; }
                                    }
                                    if (isAlreadyNot)
                                        policyDocumentView.SharePolicyDocuments.Add(sharePolicyDocument);
                                }
                            }
                        }
                        policyDocumentViews.Add(policyDocumentView);
                    }
                }
            }
            return policyDocumentViews;
        }

        private bool GetPolicyAcknowledgement(int policyDocumentId, int? EmployeeId)
        {
            return _policyAcknowledgedRepository.GetAll().Where(x => x.PolicyDocumentId == policyDocumentId && x.EmployeeId == EmployeeId)?.
                                                        Select(y => y.AcknowledgedStatus)?.FirstOrDefault()?.ToLower() == "agreed";
        }

        public int? GetPolicyAcknowledgementByEmployee(int EmployeeId, int LocationId = 0, int RoleId = 0,
                                                        int DepartmentId = 0, int CurrentWorkLocationId = 0, int CurrentWorkPlaceId = 0)
        {
            return _policyDocumentRepository.GetPolicyAcknowledgementByEmployee(EmployeeId, LocationId, RoleId,
                                                                                DepartmentId, CurrentWorkLocationId, CurrentWorkPlaceId);
        }

        public async Task<bool> SaveFolder(FolderView folderView)
        {
            Folder folder = _folderRepository.Get(folderView.FolderId);
            if (folder == null)
            {
                folder = new()
                {
                    FolderName = folderView.FolderName,
                    CreatedBy = folderView.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };
                await _folderRepository.AddAsync(folder);
            }
            else
            {
                folder.FolderName = folderView.FolderName;
                folder.ModifiedBy = folderView.ModifiedBy;
                folder.ModifiedOn = DateTime.UtcNow;
                _folderRepository.Update(folder);
            }
            await _folderRepository.SaveChangesAsync();
            return true;
        }

        public async Task<int> SavePolicyDocument(PolicyDocumentView policyDocumentView)
        {
            PolicyDocuments? policyDocuments = _policyDocumentRepository.Get(policyDocumentView.PolicyDocumentId);
            if (policyDocuments == null)
            {
                policyDocuments = new()
                {
                    FileName = policyDocumentView.FileName,
                    Description = policyDocumentView.Description,
                    FolderId = policyDocumentView.FolderId,
                    ValidTo = policyDocumentView.ValidTo,
                    Acknowledgement = policyDocumentView.Acknowledgement,
                    IsEmployeeAbleToDownload = policyDocumentView.ToDownload?.Where(x => x.Equals("employee")).Any(),
                    IsRMAbleToDownload = policyDocumentView.ToDownload?.Where(x => x.Equals("reportingManager")).Any(),
                    IsNotifyViaEmail = policyDocumentView.NotifyThrough?.Where(x => x.Equals("email")).Any(),
                    IsNotifyViaFeeds = policyDocumentView.NotifyThrough?.Where(x => x.Equals("notification")).Any(),
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = policyDocumentView.CreatedBy,
                    FilePath = policyDocumentView.FilePath,
                    ToShareWithAll = policyDocumentView.ToShareWithAll,
                    EmployeeId = policyDocumentView.EmployeeId,
                    IsEmployeeAbleToView = policyDocumentView.ToView?.Where(x => x.Equals("employee")).Any(),
                    IsRMAbleToView = policyDocumentView.ToView?.Where(x => x.Equals("reportingManager")).Any()
                };
                await _policyDocumentRepository.AddAsync(policyDocuments);
            }
            else
            {
                policyDocuments.FileName = policyDocumentView.FileName;
                policyDocuments.Description = policyDocumentView.Description;
                policyDocuments.FolderId = policyDocumentView.FolderId;
                policyDocuments.ValidTo = policyDocumentView.ValidTo;
                policyDocuments.Acknowledgement = policyDocumentView.Acknowledgement;
                policyDocuments.IsEmployeeAbleToDownload = policyDocumentView.ToDownload?.Where(x => x.Equals("employee")).Any();
                policyDocuments.IsRMAbleToDownload = policyDocumentView.ToDownload?.Where(x => x.Equals("reportingManager")).Any();
                policyDocuments.IsNotifyViaEmail = policyDocumentView.NotifyThrough?.Where(x => x.Equals("email")).Any();
                policyDocuments.IsNotifyViaFeeds = policyDocumentView.NotifyThrough?.Where(x => x.Equals("notification")).Any();
                policyDocuments.FilePath = policyDocumentView.FilePath;
                policyDocuments.ToShareWithAll = policyDocumentView.ToShareWithAll;
                policyDocuments.EmployeeId = policyDocumentView.EmployeeId;
                policyDocuments.IsEmployeeAbleToView = policyDocumentView.ToView?.Where(x => x.Equals("employee")).Any();
                policyDocuments.IsRMAbleToView = policyDocumentView.ToView?.Where(x => x.Equals("reportingManager")).Any();
                policyDocuments.ModifiedBy = policyDocumentView.ModifiedBy;
                policyDocuments.ModifiedOn = DateTime.UtcNow;
                _policyDocumentRepository.Update(policyDocuments);
            }
            await _policyDocumentRepository.SaveChangesAsync();
            //delete the existing shared items
            List<SharePolicyDocuments>? sharePolicyDocuments = _sharePolicyDocumentRepository.GetSharePolicyDocumentsByPolicyDocumentId(policyDocumentView.PolicyDocumentId);
            if (sharePolicyDocuments != null)
            {
                foreach (SharePolicyDocuments sharePolicyDocument in sharePolicyDocuments)
                {
                    _sharePolicyDocumentRepository.Delete(sharePolicyDocument);
                }
                await _sharePolicyDocumentRepository.SaveChangesAsync();
            }
            if (!(policyDocumentView.ToShareWithAll == true || policyDocumentView.EmployeeId != null))
            {
                if (policyDocumentView.SharePolicyDocuments?.Count > 0)
                {
                    foreach (SharePolicyDocumentView documentView in policyDocumentView.SharePolicyDocuments)
                    {
                        int? count = documentView.DepartmentId?.Count;
                        if (documentView.LocationId?.Count > documentView.DepartmentId?.Count)
                            count = documentView.LocationId?.Count;
                        if (documentView.RoleId?.Count > documentView.LocationId?.Count)
                            count = documentView.RoleId?.Count;
                        for (int i = 0; i < count; i++)
                        {
                            SharePolicyDocuments sharePolicyDocument = new()
                            {
                                PolicyDocumentId = policyDocuments.PolicyDocumentId,
                                DepartmentId = documentView.DepartmentId?.Count > i ? documentView.DepartmentId?[i] == null ? null : documentView.DepartmentId?[i] : null,
                                LocationId = documentView.LocationId?.Count > i ? documentView.LocationId?[i] == null ? null : documentView.LocationId?[i] : null,
                                RoleId = documentView.RoleId?.Count > i ? documentView.RoleId?[i] == null ? null : documentView.RoleId?[i] : null,
                                CreatedOn = DateTime.UtcNow,
                                CreatedBy = documentView.CreatedBy
                            };
                            await _sharePolicyDocumentRepository.AddAsync(sharePolicyDocument);
                        }
                    }
                    await _sharePolicyDocumentRepository.SaveChangesAsync();
                }
            }
            return policyDocuments.PolicyDocumentId;
        }

        public async Task<bool> DeletePolicyDocumentById(int PolicyDocumentId, int ModifiedBy, string ArchivePath)
        {
            PolicyDocuments? policyDocument = _policyDocumentRepository.GetAll().Where(x => x.PolicyDocumentId == PolicyDocumentId).FirstOrDefault();
            if (policyDocument != null)
            {
                policyDocument.ValidTo = DateTime.UtcNow;
                policyDocument.ModifiedOn = DateTime.UtcNow;
                policyDocument.ModifiedBy = ModifiedBy;
                policyDocument.FilePath = ArchivePath;
                _policyDocumentRepository.Update(policyDocument);
                await _policyDocumentRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int?> UpdatePolicyAcknowledgement(PolicyAcknowledgementView document)
        {
            PolicyAcknowledged policyAcknowledged = new()
            {
                EmployeeId = document.AcknowledgedBy,
                PolicyDocumentId = document.PolicyDocumentId,
                AcknowledgedBy = document.AcknowledgedBy,
                AcknowledgedAt = DateTime.UtcNow,
                AcknowledgedStatus = document.AcknowledgedStatus
            };
            await _policyAcknowledgedRepository.AddAsync(policyAcknowledged);
            await _policyAcknowledgedRepository.SaveChangesAsync();
            return _policyDocumentRepository.Get((int)document.PolicyDocumentId!).CreatedBy;
        }

        public PolicyEmployeeAcknowledgementListView? GetEmployeeListForAcknowledgement(int policyDocumentId)
        {
            PolicyEmployeeAcknowledgementListView policyEmployeeAcknowledgementListView = new();
            List<PolicyAcknowledged>? policyAcknowledgements = new();
            List<PolicyAcknowledgementView> policyAcknowledgementViews = new();
            List<int> RoleIds = new();
            List<int> LocationIds = new();
            List<int> DepartmentIds = new();
            policyAcknowledgements = _policyAcknowledgedRepository.GetAll().
                                Where(x => x.PolicyDocumentId == policyDocumentId).
                                OrderByDescending(y => y.AcknowledgedAt).ToList();
            foreach (PolicyAcknowledged policyAcknowledged in policyAcknowledgements)
            {
                PolicyAcknowledgementView policy = new()
                {
                    AcknowledgedAt = policyAcknowledged.AcknowledgedAt,
                    AcknowledgedBy = policyAcknowledged.AcknowledgedBy,
                    AcknowledgedStatus = policyAcknowledged.AcknowledgedStatus,
                    PolicyDocumentId = policyAcknowledged.PolicyDocumentId,
                    PolicyAcknowledgedId = policyAcknowledged.PolicyAcknowledgedId
                };
                policyAcknowledgementViews.Add(policy);
            }
            policyEmployeeAcknowledgementListView.PolicyAcknowledgements = policyAcknowledgementViews;
            policyEmployeeAcknowledgementListView.ToShareWithAll = _policyDocumentRepository.Get(policyDocumentId).ToShareWithAll;
            if (policyEmployeeAcknowledgementListView.ToShareWithAll == false)
            {
                List<SharePolicyDocuments>? sharePolicyDocuments = _sharePolicyDocumentRepository.GetAll().
                                                                        Where(x => x.PolicyDocumentId == policyDocumentId).ToList();
                if (sharePolicyDocuments?.Count() > 0)
                {
                    foreach (SharePolicyDocuments sharePolicyDocument in sharePolicyDocuments)
                    {
                        if (sharePolicyDocument.RoleId > 0) RoleIds.Add((int)sharePolicyDocument.RoleId);
                        if (sharePolicyDocument.LocationId > 0) LocationIds.Add((int)sharePolicyDocument.LocationId);
                        if (sharePolicyDocument.DepartmentId > 0) DepartmentIds.Add((int)sharePolicyDocument.DepartmentId);
                    }
                }
            }
            policyEmployeeAcknowledgementListView.RoleIds = RoleIds;
            policyEmployeeAcknowledgementListView.LocationIds = LocationIds;
            policyEmployeeAcknowledgementListView.DepartmentIds = DepartmentIds;
            return policyEmployeeAcknowledgementListView;
        }
    }
}