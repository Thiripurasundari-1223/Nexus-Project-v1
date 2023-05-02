using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Projects
{
    public class AddCRView
    {
        public ChangeRequestView ChangeRequest { get; set; }
        public List<DocumentsToUpload> ListOfDocuments { get; set; }
    }
}
