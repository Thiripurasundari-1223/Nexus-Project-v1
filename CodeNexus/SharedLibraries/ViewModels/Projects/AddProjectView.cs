using SharedLibraries.ViewModels.Notifications;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Projects
{
    public class AddProjectView
    {
        public ProjectDetailView ProjectDetail { get; set; }
        public List<DocumentsToUpload> ListOfDocuments { get; set; }
    }
}
