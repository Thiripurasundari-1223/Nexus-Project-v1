using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Employees
{
    public class ADUserList
    {
        public string givenName { get; set; }
        public string mail { get; set; }
        public string mobilePhone { get; set; }
        public string officeLocation { get; set; }
        public string surname { get; set; }
        public string id { get; set; }
        public string jobTitle { get; set; }
        public string userPrincipalName { get; set; }
    }
    public class ADUserToken
    {
        public string authToken { get; set; }
        public int userId { get; set; }
        public int? pShiftDetailsId { get; set; }
    }
}