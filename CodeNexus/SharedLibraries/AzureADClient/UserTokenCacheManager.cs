using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SharedLibraries.AzureADClient
{
    public static class UserTokenCacheManager
    {
        private static readonly List<UserTokenCache> userTokenCacheItems = new List<UserTokenCache>();
        public static void AddUserTokenCache(UserTokenCache userTokenCache) { userTokenCacheItems.Add(userTokenCache); }
        public static void RemoveUserTokenCache(string accessToken)
        {
            if (userTokenCacheItems.Any(x => x.AccessToken == accessToken))
            {
                userTokenCacheItems.Remove(userTokenCacheItems.FirstOrDefault(x => x.AccessToken == accessToken));
            }
        }
        public static void RemoveUserTokenCacheByTokenID(string IDToken)
        {
            if (userTokenCacheItems.Any(x => x.IDToken == IDToken))
            {
                userTokenCacheItems.Remove(userTokenCacheItems.FirstOrDefault(x => x.IDToken == IDToken));
            }
        }
        public static UserTokenCache GetUserTokenCache(string accessToken)
        {
            return userTokenCacheItems.FirstOrDefault(x => x.AccessToken == accessToken);
        }
        public static UserTokenCache GetUserIDTokenCache(string IDToken)
        {
            return userTokenCacheItems.FirstOrDefault(x => x.IDToken == IDToken);
        }
    }
    public static class UserTokenValidateCacheManager
    {
        private static readonly List<UserValidateTokenCache> userValidateTokenCacheItems = new List<UserValidateTokenCache>();
        public static void AddUserValidateTokenCache(string authToken, ClaimsPrincipal claimsPrincipal, SecurityToken validateToken)
        {
            if (userValidateTokenCacheItems.Any(x => x.AuthToken == authToken))
            {
                userValidateTokenCacheItems.Remove(userValidateTokenCacheItems.FirstOrDefault(x => x.AuthToken == authToken));
            }
            userValidateTokenCacheItems.Add(new UserValidateTokenCache
            {
                AuthToken = authToken,
                ClaimsPrincipal = claimsPrincipal,
                ValidateToken = validateToken
            });
        }
        public static UserValidateTokenCache GetUserValidateTokenCache(string authToken)
        {
            if (userValidateTokenCacheItems.Any(x => x.AuthToken == authToken))
            {
                var userValidateTokenCache = userValidateTokenCacheItems.FirstOrDefault(x => x.AuthToken == authToken);
                if (userValidateTokenCache.ValidateToken.ValidTo > DateTime.UtcNow) { return userValidateTokenCache; }
                else return null;
            }
            else return null;
        }
    }
    public class UserTokenCache
    {
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string IDToken { get; set; }
        public DateTime ExpirationTime { get; set; }
        public List<AppTokenCache> AppTokenCaches { get; set; }
    }
    public class AppTokenCache
    {
        public string AppToken { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
    public class UserValidateTokenCache
    {
        public string AuthToken { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        public SecurityToken ValidateToken { get; set; }
    }
}