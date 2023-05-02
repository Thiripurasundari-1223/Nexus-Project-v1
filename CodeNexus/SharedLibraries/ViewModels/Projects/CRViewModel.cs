using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Projects
{
    public class CRViewModel
    {
        public ChangeRequestView CRDetails { get; set; }
        public List<ProjectDetailCommentsList> CRCommentsList { get; set; }
    }
}
