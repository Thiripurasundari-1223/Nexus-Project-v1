using PolicyManagement.DAL.DBContext;
using PolicyManagement.DAL.Models;

namespace PolicyManagement.DAL.Repository
{
    public interface IAnnouncementRepository : IBaseRepository<Announcement>
    {
    }
    public class AnnouncementRepository : BaseRepository<Announcement>, IAnnouncementRepository
    {
        private readonly PolicyMgmtDBContext _dbContext;
        public AnnouncementRepository(PolicyMgmtDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}