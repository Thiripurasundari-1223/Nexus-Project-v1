using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnBoarding.DAL.DBContext;
using SharedLibraries.Models.Accounts;
using SharedLibraries.ViewModels.Accounts;

namespace CustomerOnBoarding.DAL.Repository
{
    public interface ICustomerContactDetailsRepository : IBaseRepository<CustomerContactDetails>
    {
        public List<CustomerContactDetails> GetByID(int pAccountID);
        List<CustomerContactDetailsView> GetCustomerContactInformationByAccountId(int pAccountID, int versionId, bool isLastVersion);
    }
    public class CustomerContactDetailsRepository : BaseRepository<CustomerContactDetails>, ICustomerContactDetailsRepository
    {
        private readonly COBDBContext dbContext;
        public CustomerContactDetailsRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<CustomerContactDetails> GetByID(int pAccountID)
        {
            return dbContext.CustomerContactDetails.Where(x => x.AccountId == pAccountID).ToList();
        }
        public List<CustomerContactDetailsView> GetCustomerContactInformationByAccountId(int pAccountID, int versionId, bool isLastVersion)
        {
            if(isLastVersion)
            {
                return dbContext.CustomerContactDetails.Where(x => x.AccountId == pAccountID).Select(rs => new CustomerContactDetailsView
                {
                    CustomerContactDetailId = rs.CustomerContactDetailId,
                    AccountId = rs.AccountId,
                    ContactPersonFirstName = rs.ContactPersonFirstName,
                    ContactPersonLastName = rs.ContactPersonLastName,
                    ContactPersonEmailAddress = rs.ContactPersonEmailAddress,
                    ContactPersonPhoneNumber = rs.ContactPersonPhoneNumber,
                    DesignationName = rs.DesignationName,
                    CountryId = rs.CountryId,
                    AddressDetail = rs.AddressDetail,
                    CityName = rs.CityName,
                    StateId = rs.StateId,
                    Postalcode = rs.Postalcode,
                    CountryName= dbContext.Country.Where(x => x.CountryId == rs.CountryId).Select(x => x.CountryName).FirstOrDefault(),
                    StateName= dbContext.State.Where(x => x.StateId == rs.StateId).Select(x => x.StateName).FirstOrDefault()
                }).ToList();
            }
            else
            {
                return dbContext.VersionCustomerContactDetails.Where(x => x.VersionId == versionId).Select(rs => new CustomerContactDetailsView
                {
                    CustomerContactDetailId = rs.VersionCustomerContactDetailId,
                    AccountId = pAccountID,
                    ContactPersonFirstName = rs.ContactPersonFirstName,
                    ContactPersonLastName = rs.ContactPersonLastName,
                    ContactPersonEmailAddress = rs.ContactPersonEmailAddress,
                    ContactPersonPhoneNumber = rs.ContactPersonPhoneNumber,
                    DesignationName = rs.DesignationName,
                    CountryId = rs.CountryId,
                    AddressDetail = rs.AddressDetail,
                    CityName = rs.CityName,
                    StateId = rs.StateId,
                    Postalcode = rs.Postalcode
                }).ToList();
            }
            
        }
    }
}
