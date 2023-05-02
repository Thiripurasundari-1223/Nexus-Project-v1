using SharedLibraries.Models.Accounts;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Accounts
{
    public class AddAccountView
    {
       public AccountDetails AccountDetails { get; set; }
       public List<CustomerContactDetails> CustomerContactDetails { get; set; }
       public List<DocumentsToUpload> ListOfDocuments { get; set; }
        public string CreatedByName { get; set; }
    }
}
