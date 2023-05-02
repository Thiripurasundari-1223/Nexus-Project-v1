using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class NexusInfo
    {
        [Key]
        public int Id { get; set; }
        public int NoOfAccounts { get; set; }
        public int NoOfProjects { get; set; }
        public int NoOfUsers { get; set; }
    }
}
