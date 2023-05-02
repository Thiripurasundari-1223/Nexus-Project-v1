using Microsoft.AspNetCore.Http;
using Notifications.DAL.Repository;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.DAL.Services
{
    public class NotificationsServices
    {
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly INotificationsRepository _notificationsRepository;
        private readonly ISupportingDocumentsRepository _supportingDocumentsRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ITimesheetConfigurationWeekdayRepository _timesheetConfigurationWeekdayRepository;

        public NotificationsServices(INotificationsRepository notificationsRepository, ISupportingDocumentsRepository supportingDocumentsRepository
            , IStatusRepository statusRepository, ITimesheetConfigurationWeekdayRepository timesheetConfigurationWeekdayRepository)
        {
            _notificationsRepository = notificationsRepository;
            _supportingDocumentsRepository = supportingDocumentsRepository;
            _statusRepository = statusRepository;
            _timesheetConfigurationWeekdayRepository = timesheetConfigurationWeekdayRepository;
        }

        #region Insert Notifications
        public async Task<bool> InsertNotifications(List<SharedLibraries.Models.Notifications.Notifications> pNotifications)
        {
            try
            {
                foreach (SharedLibraries.Models.Notifications.Notifications notification in pNotifications)
                {
                    SharedLibraries.Models.Notifications.Notifications notifications = new SharedLibraries.Models.Notifications.Notifications
                    {
                        CreatedBy = notification.CreatedBy,
                        CreatedOn = DateTime.UtcNow,
                        FromId = notification.FromId,
                        ToId = notification.ToId,
                        MarkAsRead = false,
                        NotificationSubject = notification.NotificationSubject,
                        NotificationBody = notification.NotificationBody,
                        PrimaryKeyId = notification.PrimaryKeyId,
                        ButtonName = notification.ButtonName,
                        SourceType = notification.SourceType,
                        Data = notification.Data
                    };
                    await _notificationsRepository.AddAsync(notifications);
                }
                await _notificationsRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get Notifications By Resource Id
        public List<NotificationView> GetNotificationsByResourceId(int pResourceId)
        {
            return _notificationsRepository.GetNotificationsByResourceId(pResourceId);
        }
        #endregion

        #region Get Notification UnRead By Resource Id
        public int GetNotificationUnReadByResourceId(int pResourceId)
        {
            return _notificationsRepository.GetNotificationUnReadByResourceId(pResourceId);
        }
        #endregion

        #region Mark All As Read
        public async Task<bool> MarkAllAsRead(int pResourceId)
        {
            try
            {
                List<SharedLibraries.Models.Notifications.Notifications> notifications = _notificationsRepository.GetListNotificationUnReadByResourceId(pResourceId);
                foreach (SharedLibraries.Models.Notifications.Notifications notification in notifications)
                {
                    notification.MarkAsRead = true;
                    notification.ModifiedBy = pResourceId;
                    notification.ModifiedOn = DateTime.UtcNow;
                    _notificationsRepository.Update(notification);
                }
                await _notificationsRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Mark As Read By Notification Id
        public async Task<bool> MarkAsReadByNotificationId(int pNotificationId, int pResourceId)
        {
            SharedLibraries.Models.Notifications.Notifications notifications = _notificationsRepository.Get(pNotificationId);
            if (notifications != null)
            {
                notifications.ModifiedBy = pResourceId;
                notifications.ModifiedOn = DateTime.UtcNow;
                notifications.MarkAsRead = true;
                _notificationsRepository.Update(notifications);
                await _notificationsRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion

        #region Add supporting documents
        public async Task<bool> AddSupportingDocuments(SupportingDocumentsView supportingDocuments)
        {
            try
            {
                string directoryPath = Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.SourceId.ToString());               
                //Upload new files
                foreach (var document in supportingDocuments.ListOfDocuments)
                {                    
                    SupportingDocuments supDocument = new SupportingDocuments
                    {
                        SourceId = supportingDocuments.SourceId,
                        SourceType = supportingDocuments.SourceType,
                        DocumentCategory = document.DocumentCategory,
                        IsApproved = document.IsApproved,
                        DocumentName = document.DocumentName,
                        DocumentPath = directoryPath,
                        DocumentType = document.DocumentType,
                        DocumentSize = document.DocumentSize,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = supportingDocuments.CreatedBy
                    };
                    await _supportingDocumentsRepository.AddAsync(supDocument);
                }
                await _supportingDocumentsRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
                //logger.Error(ex.Message.ToString());
            }
            //return false;
        }
        #endregion
        //#region Delete existing documents
        //public async Task<bool> DeleteSupportingDocuments(SupportingDocumentsView supportingDocumentsView)
        //{
        //    try
        //    {
        //        string directoryPath = Path.Combine(supportingDocumentsView.BaseDirectory, supportingDocumentsView.SourceType, supportingDocumentsView.SourceId.ToString());
        //        Directory.CreateDirectory(directoryPath);
        //        //Delete eisting files
        //        var lstDocuments = _supportingDocumentsRepository.GetDocumentBySourceIdAndType(supportingDocumentsView.SourceId, supportingDocumentsView.SourceType);
        //        if (lstDocuments?.Count > 0)
        //        {
        //            foreach (var document in lstDocuments)
        //            {
        //                SupportingDocuments supDocument = _supportingDocumentsRepository.GetByID(document.DocumentId);
        //                if (supDocument?.DocumentId > 0)
        //                {
        //                    _supportingDocumentsRepository.Delete(supDocument);
        //                }
        //            }
        //            await _supportingDocumentsRepository.SaveChangesAsync();
        //            DirectoryInfo directory = new DirectoryInfo(directoryPath);
        //            foreach (FileInfo file in directory.GetFiles())
        //            {
        //                file.Delete();
        //            }
        //            //foreach (DirectoryInfo dir in directory.GetDirectories())
        //            //{
        //            //    dir.Delete(true);
        //            //}
        //        }               
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        logger.Error(ex.Message.ToString());
        //    }
        //    return false;
        //}
        //#endregion
        #region Delete document by id
        public async Task<bool> DeleteDocumentById(int documentId)
        {
            try
            {
                if (documentId > 0)
                {
                    SupportingDocuments supDocument = _supportingDocumentsRepository.GetByID(documentId);
                    if (supDocument?.DocumentId > 0)
                    {
                        _supportingDocumentsRepository.Delete(supDocument);
                        await _supportingDocumentsRepository.SaveChangesAsync();

                        //Delete file from physical directory
                        string filePath = Path.Combine(supDocument.DocumentPath, supDocument.DocumentName);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        return true;
                    }                    
                }                
            }
            catch (Exception)
            {
                throw;
                //logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion

        #region Get documents by Id
        public List<SupportingDocuments> GetDocumentBySourceIdAndType(SourceDocuments sourceDocuments)
        {
            List<SupportingDocuments> lstOfDocument = new List<SupportingDocuments>();
            try
            {                
                if (sourceDocuments !=null)
                {
                    lstOfDocument = _supportingDocumentsRepository.GetDocumentBySourceIdAndType(sourceDocuments);
                    
                }
            }
            catch (Exception)
            {
                throw;
                //logger.Error(ex.Message.ToString());
            }
            return lstOfDocument;
        }
        #endregion
        #region Download document
        public SupportingDocuments DownloadDocumentById(int documentId)
        {
            SupportingDocuments documentDetails = new SupportingDocuments();
            try
            {
                if (documentId > 0)
                {
                    documentDetails = _supportingDocumentsRepository.GetByID(documentId);                    
                }
            }
            catch (Exception)
            {
                throw;
                //logger.Error(ex.Message.ToString());
            }
            return documentDetails;
        }
        #endregion

        #region All Status 
        public List<Status> GetAllStatus()
        {
            return _statusRepository.GetAllStatus();
        }
        #endregion

        #region All Status List
        public List<StatusViewModel> GetAllStatusList()
        {
            return _statusRepository.GetAllStatusList();
        }
        #endregion

        #region Get Status By ID
        public StatusViewModel GetStatusByID(int pStatusId)
        {
            return _statusRepository.GetStatusByID(pStatusId);
        }
        #endregion

        #region Get Status By Name
        public StatusViewModel GetStatusByName(string pStatusName)
        {
            return _statusRepository.GetStatusByName(pStatusName);
        }
        #endregion

        #region Get Status By Code
        public StatusViewModel GetStatusByCode(string pStatusCode)
        {
            return _statusRepository.GetStatusByCode(pStatusCode);
        }
        #endregion


        #region Approved the documents by source id and type
        public async Task<bool> ApprovedDocumentsBySourceIdAndType(SourceDocuments sourceDocuments)
        {
            List<SupportingDocuments> lstOfDocument = new List<SupportingDocuments>();
            try
            {
                if (sourceDocuments != null)
                {
                    lstOfDocument = _supportingDocumentsRepository.GetDocumentBySourceIdAndType(sourceDocuments);
                    foreach (var supDocument in lstOfDocument)
                    {
                        supDocument.IsApproved = true;
                        _supportingDocumentsRepository.Update(supDocument);
                    }
                    await _supportingDocumentsRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
                //logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion
        #region Get Status By Code
        public List<StatusViewModel> GetStatusByCode(List<string> pStatusCode)
        {
            return _statusRepository.GetStatusByCode(pStatusCode);
        }
        #endregion

        #region All WeekDay List
        public List<TimesheetConfigurationWeekdayView> GetAllWeekDayList()
        {
            return _timesheetConfigurationWeekdayRepository.GetAllWeekDayList();
        }
        #endregion
        #region Get WeekDay By ID
        public TimesheetConfigurationWeekdayView GetWeekDayByID(int pTimesheetConfigurationWeekdayId)
        {
            return _timesheetConfigurationWeekdayRepository.GetWeekDayByID(pTimesheetConfigurationWeekdayId);
        }
        #endregion
        #region Get WeekDay By Name
        public TimesheetConfigurationWeekdayView GetWeekDayByName(string PDayName)
        {
            return _timesheetConfigurationWeekdayRepository.GetWeekDayByName(PDayName);
        }
        #endregion


    }
}