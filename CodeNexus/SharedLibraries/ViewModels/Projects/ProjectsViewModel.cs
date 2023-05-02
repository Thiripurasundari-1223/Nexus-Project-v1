using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Projects
{
    public class ProjectsViewModel
    {
        public ProjectDetailView ProjectDetails { get; set; }
        public List<ProjectDetailCommentsList> ProjectDetailsCommentsList { get; set; }
        public List<ChangeRequestView> ChangeRequestList { get; set; }
    }
}
