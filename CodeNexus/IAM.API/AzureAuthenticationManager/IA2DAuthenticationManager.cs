using IAM.DAL.DBContext;

namespace IAM.API.AzureAuthenticationManager
{
    public interface IA2DAuthenticationManager
    {
        string Authenticate(IAMDBContext iAMDBContext, string pEmailAddress, string Password);
    }
}