using PolicyManagement.DAL.Models;
using PolicyManagement.DAL.Repository;
using SharedLibraries.ViewModels.PolicyManagement;

namespace PolicyManagement.DAL.Services
{
    public class RequestedDocumentService
    {
        private readonly IRequestedDocumentRepository _requestedDocumentRepository;
        private readonly IDocumentTypesRepository _documentTypesRepository;
        private readonly IDocumentTagRepository _documentTagRepository;

        public RequestedDocumentService(IRequestedDocumentRepository requestedDocumentRepository,
            IDocumentTypesRepository documentTypesRepository, IDocumentTagRepository documentTagRepository)
        {
            _requestedDocumentRepository = requestedDocumentRepository;
            _documentTypesRepository = documentTypesRepository;
            _documentTagRepository = documentTagRepository;
        }

        public List<EmployeeListForRequestedDocuments> GetEmployeeListForRequestedDocuments(string status)
        {
            List<EmployeeListForRequestedDocuments> employeeLists = new();
            List<RequestedDocuments>? requestedDocuments = null;
            if (string.IsNullOrEmpty(status) || status.ToLower() == "all")
                requestedDocuments = _requestedDocumentRepository.GetAll().DistinctBy(x => x.CreatedBy).ToList();
            else
                requestedDocuments = _requestedDocumentRepository.GetAll().Where(x => x.Status?.ToLower() == status.ToLower()).
                                            DistinctBy(x => x.CreatedBy).ToList();
            foreach (RequestedDocuments documentType in requestedDocuments)
            {
                EmployeeListForRequestedDocuments employee = new()
                {
                    EmployeeId = documentType.CreatedBy,
                    IsRequestPending = _requestedDocumentRepository.GetAll().
                                                                 Where(x => x.CreatedBy == documentType.CreatedBy &&
                                                                 x.Status?.ToLower() == "pending").Count() > 0 ? true : false
                };
                employeeLists.Add(employee);
            }
            return employeeLists;
        }

        public List<RequestedDocumentView>? GetRequestedDocuments(int UserId = 0)
        {
            List<RequestedDocuments>? requestedDocuments = new();
            if (UserId > 0)
            {
                requestedDocuments = _requestedDocumentRepository.GetAll().Where(x => x.CreatedBy == UserId).ToList();
            }
            else
                requestedDocuments = _requestedDocumentRepository.GetAll().ToList();
            if (requestedDocuments == null || requestedDocuments.Count == 0) return null;
            List<RequestedDocumentView> requestedDocumentViews = new();
            List<DocumentTypes> documentTypes = _documentTypesRepository.GetAll().ToList();
            foreach (RequestedDocuments documentType in requestedDocuments)
            {
                RequestedDocumentView requestedDocumentView = new()
                {
                    ApprovedOrRejectedBy = documentType.ApprovedOrRejectedBy,
                    ApprovedOrRejectedOn = documentType.ApprovedOrRejectedOn,
                    CreatedBy = documentType.CreatedBy,
                    CreatedOn = documentType.CreatedOn,
                    DocumentTypeId = documentType.DocumentTypeId,
                    DocumentType = documentTypes.Where(x => x.DocumentTypeId == documentType.DocumentTypeId).Select(x => x.DocumentType).FirstOrDefault(),
                    ModifiedBy = documentType.ModifiedBy,
                    ModifiedOn = documentType.ModifiedOn,
                    Reason = documentType.Reason,
                    RejectedReason = documentType.RejectedReason,
                    RequestedDocumentId = documentType.RequestedDocumentId,
                    Status = documentType.Status,
                    PolicyDocumentId = documentType.PolicyDocumentId
                };
                requestedDocumentViews.Add(requestedDocumentView);
            }
            return requestedDocumentViews;
        }

        public List<DocumentTypeView> GetDocumentTypes(int documentTypeId = 0)
        {
            List<DocumentTypeView> documentTypesView = new();
            List<DocumentTypes> documentTypes = new();
            List<DocumentTagView> documentTagViews = new();
            List<DocumentTag> documentTags = new();
            if (documentTypeId == 0)
            {
                documentTypes = _documentTypesRepository.GetAll().ToList();
                documentTags = _documentTagRepository.GetAll().ToList();
            }
            else
            {
                DocumentTypes document = _documentTypesRepository.Get(documentTypeId);
                documentTypes.Add(document);
            }
            foreach (DocumentTypes documentType in documentTypes)
            {
                DocumentTypeView documentTypeView = new()
                {
                    CreatedBy = documentType.CreatedBy,
                    CreatedOn = documentType.CreatedOn,
                    DocumentTypeId = documentType.DocumentTypeId,
                    DocumentType = documentType.DocumentType,
                    ModifiedBy = documentType.ModifiedBy,
                    ModifiedOn = documentType.ModifiedOn,
                    ExistingFilePath = documentType.TemplateFile,
                    TemplateFile = documentType.TemplateFile,
                    SignatureFile = documentType.SignatureFile
                };
                documentTypesView.Add(documentTypeView);
            }
            if (documentTypeId == 0 || documentTypesView.Count == 0)
            {
                foreach (DocumentTag documentTag in documentTags)
                {
                    DocumentTagView documentTagView = new()
                    {
                        CreatedBy = documentTag.CreatedBy,
                        CreatedOn = documentTag.CreatedOn,
                        DocumentTagId = documentTag.DocumentTagId,
                        ModifiedBy = documentTag.ModifiedBy,
                        ModifiedOn = documentTag.ModifiedOn,
                        TagName = documentTag.TagName,
                        PlaceHolderName = documentTag.PlaceHolderName
                    };
                    documentTagViews.Add(documentTagView);
                }
                if (documentTypesView.Count == 0)
                {
                    documentTypesView = new()
                    {
                        new()
                        {
                            DocumentTag = documentTagViews
                        }
                    };
                }
                else
                    documentTypesView.FirstOrDefault()!.DocumentTag = documentTagViews;
            }
            return documentTypesView;
        }

        public async Task<int> SaveRequestedDocument(RequestedDocumentView requestedDocument)
        {
            RequestedDocuments document = _requestedDocumentRepository.Get(requestedDocument.RequestedDocumentId);
            if (document == null)
            {
                document = new()
                {
                    DocumentTypeId = requestedDocument.DocumentTypeId,
                    OtherDocumentType = requestedDocument.OtherDocumentType,
                    Reason = requestedDocument.Reason,
                    Status = requestedDocument.Status,
                    CreatedBy = requestedDocument.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    PolicyDocumentId = requestedDocument.PolicyDocumentId
                };
                await _requestedDocumentRepository.AddAsync(document);
            }
            else
            {
                document.DocumentTypeId = requestedDocument.DocumentTypeId;
                document.OtherDocumentType = requestedDocument.OtherDocumentType;
                document.Reason = requestedDocument.Reason;
                document.Status = requestedDocument.Status;
                document.ModifiedBy = requestedDocument.ModifiedBy;
                document.ModifiedOn = DateTime.UtcNow;
                _requestedDocumentRepository.Update(document);
            }
            await _requestedDocumentRepository.SaveChangesAsync();
            return document.RequestedDocumentId;
        }

        public async Task<bool> ApproveOrRejectRequestedDocument(RequestedDocumentView requestedDocument)
        {
            RequestedDocuments document = _requestedDocumentRepository.Get(requestedDocument.RequestedDocumentId);
            if (document != null)
            {
                document.Status = requestedDocument.Status;
                document.ApprovedOrRejectedBy = requestedDocument.ApprovedOrRejectedBy;
                if (requestedDocument.Status?.ToLower() == "rejected")
                    document.RejectedReason = requestedDocument.RejectedReason;
                document.ApprovedOrRejectedOn = DateTime.UtcNow;
                if (requestedDocument.Status?.ToLower() == "approved")
                    document.PolicyDocumentId = requestedDocument.PolicyDocumentId;
                _requestedDocumentRepository.Update(document);
                await _requestedDocumentRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public RequestedDocumentView GenerateRequestedDocument(int requestedDocumentId)
        {
            RequestedDocuments document = _requestedDocumentRepository.Get(requestedDocumentId);
            RequestedDocumentView documentView = new();
            if (document != null)
            {
                DocumentTypes documentType = _documentTypesRepository.Get((int)document.DocumentTypeId!);
                documentView = new()
                {
                    OtherDocumentType = document.OtherDocumentType,
                    Status = document.Status,
                    Reason = document.Reason,
                    RequestedDocumentId = document.RequestedDocumentId,
                    DocumentType = documentType.DocumentType,
                    DocumentFilePath = documentType.TemplateFile,
                    DocumentTypeId = document.DocumentTypeId,
                    ApprovedOrRejectedBy = document.ApprovedOrRejectedBy,
                    ApprovedOrRejectedOn = document.ApprovedOrRejectedOn,
                    CreatedBy = document.CreatedBy,
                    PolicyDocumentId = document.PolicyDocumentId,
                    SignatureFilePath = documentType.SignatureFile
                };
                List<PlaceHolderValue> placeHolderValues = new();
                List<DocumentTag> documentTags = _documentTagRepository.GetAll().ToList();
                foreach (DocumentTag tag in documentTags)
                {
                    PlaceHolderValue PlaceHolderValue = new()
                    {
                        Name = tag.PlaceHolderName,
                        TagName = tag.TagName
                    };
                    placeHolderValues.Add(PlaceHolderValue);
                }
                documentView.PlaceHolderValues = placeHolderValues;
                return documentView;
            }
            return documentView;
        }

        public async Task<int> SaveDocumentType(DocumentTypeView document)
        {
            DocumentTypes documentType = _documentTypesRepository.Get(document.DocumentTypeId);
            if (documentType == null)
            {
                documentType = new()
                {
                    DocumentType = document.DocumentType,
                    TemplateFile = document.TemplateFile,
                    SignatureFile = document.SignatureFile,
                    CreatedBy = document.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };
                await _documentTypesRepository.AddAsync(documentType);
            }
            else
            {
                documentType.DocumentType = document.DocumentType;
                documentType.TemplateFile = document.TemplateFile;
                documentType.SignatureFile = document.SignatureFile;
                documentType.ModifiedBy = document.ModifiedBy;
                documentType.ModifiedOn = DateTime.UtcNow;
                _documentTypesRepository.Update(documentType);
            }
            await _documentTypesRepository.SaveChangesAsync();
            return documentType.DocumentTypeId;
        }
    }
}