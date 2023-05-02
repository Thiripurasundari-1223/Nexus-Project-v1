using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients(IConfiguration config) =>
            new Client[]
            {
                   new Client
                   {
                        ClientId = "nexus_ui",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        //AbsoluteRefreshTokenLifetime = 3600,
                        //RefreshTokenUsage = TokenUsage.ReUse,
                        ClientSecrets =
                        {
                            new Secret("Nexus@Tvsn2021".Sha256())
                        },
                         AllowedScopes = new List<string> {
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile,
                               // IdentityServerConstants.StandardScopes.OfflineAccess,
                                "NexusAPI"
                         },
                        AccessTokenLifetime = 3888000 // 45 days
                      //  AbsoluteRefreshTokenLifetime = 60,
                      //  RefreshTokenExpiration = TokenExpiration.Sliding,
                      //  RefreshTokenUsage = TokenUsage.OneTimeOnly,
                      //   SlidingRefreshTokenLifetime = 1200,
                      //  AllowOfflineAccess = true,
                      //  UpdateAccessTokenClaimsOnRefresh =true,
                      //  RedirectUris = {"https://localhost:44001/home/claims",
                      //"https://localhost:44001/"}
                   }
            };


        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
               new ApiScope("NexusAPI", "Nexus API")
           };

        public static IEnumerable<ApiResource> ApiResources => new[]
        {
             new ApiResource()
        {
            Name = "NexusAPI",   //This is the name of the API
            Description = "This is the Nexus Api-resource description",
            Enabled = true,
            DisplayName = "Nexus API Service",
            Scopes = new List<string> { "NexusAPI" }

        }
            };



        public static IEnumerable<IdentityResource> IdentityResources =>
          new IdentityResource[]
          {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Address(),
              new IdentityResources.Email(),
              new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>() { "role" })
          };

        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "mehmet",
                    Password = "swn",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "mehmet"),
                        new Claim(JwtClaimTypes.FamilyName, "ozkaya")
                    }
                }
            };
    }
}