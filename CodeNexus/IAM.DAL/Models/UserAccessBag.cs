using SharedLibraries.ViewModels.Employees;
using System.ComponentModel.DataAnnotations;

namespace IAM.DAL.Models
{
    public class UserAccessBag
    {
        [Key]
        public int UserAccessBagID { get; set; }
        public User User { get; set; }
        public UserRoles UserRoles { get; set; }
        public RoleSetup RoleSetup { get; set; }
        public UserToken AccessToken { get; set; }

    }
}