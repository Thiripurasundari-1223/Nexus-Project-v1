using ProjectManagement.DAL.DBContext;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.DAL.Repository
{
    public interface IProjectDetailCommentsRepository : IBaseRepository<ProjectDetailComments>
    {
        Task Add(VersionProjectDetailComments versionProjectDetailComments);
        ProjectDetailComments GetByID(int pID);
        List<ProjectDetailCommentsList> GetProjectCommentsByProjectId(int pProjectDetailID);
    }
    public class ProjectDetailCommentsRepository : BaseRepository<ProjectDetailComments>, IProjectDetailCommentsRepository
    {
        private readonly PMDBContext dbContext;
        public ProjectDetailCommentsRepository(PMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public ProjectDetailComments GetByID(int pID)
        {
            return dbContext.ProjectDetailComments.Where(x => x.ProjectDetailCommentId == pID).FirstOrDefault();
        }
        public List<ProjectDetailCommentsList> GetProjectCommentsByProjectId(int pProjectDetailID)
        {
            List<ProjectDetailCommentsList> ProjectDetailCommentsLists = new List<ProjectDetailCommentsList>();
            List<ProjectDetailComments> ProjectDetailComments = dbContext.ProjectDetailComments.Where(x => x.ProjectId == pProjectDetailID).ToList();
            foreach (ProjectDetailComments accComments in ProjectDetailComments.OrderByDescending(x => x.ProjectDetailCommentId))
            {
                ProjectDetailCommentsList ProjectDetailCommentsList = new ProjectDetailCommentsList
                {
                    ProjectDetailCommentId = accComments.ProjectDetailCommentId,
                    ProjectDetailId = accComments.ProjectId,
                    CreatedByName ="",
                    Comments = accComments.Comments,
                    CreatedBy = accComments.CreatedBy,
                    CreatedOn = accComments.CreatedOn,
                    ModifiedBy = accComments.ModifiedBy,
                    ModifiedOn = accComments.ModifiedOn
                };
                ProjectDetailCommentsLists.Add(ProjectDetailCommentsList);
            }
            return ProjectDetailCommentsLists;
        }

        Task IProjectDetailCommentsRepository.Add(VersionProjectDetailComments versionProjectDetailComments)
        {
            var data =  dbContext.VersionProjectDetailComments.Add(versionProjectDetailComments);
            return null;
        }

       

        List<ProjectDetailCommentsList> IProjectDetailCommentsRepository.GetProjectCommentsByProjectId(int pProjectDetailID)
        {
            throw new System.NotImplementedException();
        }
    }
}