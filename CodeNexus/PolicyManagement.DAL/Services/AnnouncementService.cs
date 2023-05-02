using PolicyManagement.DAL.Models;
using PolicyManagement.DAL.Repository;
using SharedLibraries.ViewModels.PolicyManagement;

namespace PolicyManagement.DAL.Services
{
    public class AnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;
        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<string> SaveAnnouncement(AnnouncementView announcementView)
        {
            Announcement? announcement = _announcementRepository.Get(announcementView.AnnouncementId);
            if (announcement == null)
            {
                announcement = new()
                {
                    CreatedBy = announcementView.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    Description = announcementView.Description,
                    ExpiryDate = announcementView.ExpiryDate,
                    Image = announcementView.Image,
                    Subject = announcementView.Subject,
                    Topic = announcementView.Topic
                };
                await _announcementRepository.AddAsync(announcement);
            }
            else
            {
                announcement.Description = announcementView.Description;
                announcement.Topic = announcementView.Topic;
                announcement.ExpiryDate = announcementView.ExpiryDate;
                announcement.Subject = announcementView.Subject;
                announcement.Image = announcementView.Image;
                announcement.ModifiedBy = announcementView.ModifiedBy;
                announcement.ModifiedOn = DateTime.UtcNow;
                _announcementRepository.Update(announcement);
            }
            await _announcementRepository.SaveChangesAsync();
            return announcement.AnnouncementId.ToString();
        }

        public List<AnnouncementView> GetAnnouncements(int pAnnouncementId = 0)
        {
            List<AnnouncementView> announcementViews = new();
            if (pAnnouncementId > 0)
            {
                Announcement? announcement = _announcementRepository.Get(pAnnouncementId);
                if (announcement != null)
                {
                    AnnouncementView announcementView = new()
                    {
                        AnnouncementId = announcement.AnnouncementId,
                        CreatedBy = announcement.CreatedBy,
                        Description = announcement.Description,
                        CreatedOn = announcement.CreatedOn,
                        ExpiryDate = announcement.ExpiryDate,
                        Image = announcement.Image,
                        ModifiedBy = announcement.ModifiedBy,
                        ModifiedOn = announcement.ModifiedOn,
                        Subject = announcement.Subject,
                        Topic = announcement.Topic
                    };
                    announcementViews.Add(announcementView);
                }
            }
            else
            {
                List<Announcement>? announcements = _announcementRepository.GetAll().
                                                    Where(x => x.ExpiryDate == null ||
                                                    (x.ExpiryDate != null && x.ExpiryDate.Value.Date >= DateTime.UtcNow.Date)).ToList();
                if (announcements?.Count > 0)
                {
                    foreach (Announcement announcement in announcements)
                    {
                        AnnouncementView announcementView = new()
                        {
                            AnnouncementId = announcement.AnnouncementId,
                            CreatedBy = announcement.CreatedBy,
                            Description = announcement.Description,
                            CreatedOn = announcement.CreatedOn,
                            ExpiryDate = announcement.ExpiryDate,
                            Image = announcement.Image,
                            ModifiedBy = announcement.ModifiedBy,
                            ModifiedOn = announcement.ModifiedOn,
                            Subject = announcement.Subject,
                            Topic = announcement.Topic
                        };
                        announcementViews.Add(announcementView);
                    }
                }
            }
            return announcementViews;
        }
    }
}