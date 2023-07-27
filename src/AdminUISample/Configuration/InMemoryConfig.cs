using Dynami.IdentityServer4.Models;
using Dynami.IdentityServer4.Test;
using Dynami.IdentityServer4;
using IdentityModel;
using System.Security.Claims;

namespace AdminUISample.Configuration
{
    public static class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
          new List<IdentityResource>
          {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Address(),
            new IdentityResource("subject", "User subject", new List<string> { "subject" }),
            new IdentityResource("roles", "User role(s)", new List<string> { "role" }),
            new IdentityResource("position", "Your position", new List<string> { "position" }),
            new IdentityResource("country", "Your country", new List<string> { "country" })
          };

        public static List<TestUser> GetUsers() =>
          new List<TestUser>
          {
              new TestUser
              {
                  SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
                  Username = "Mick",
                  Password = "MickPassword",
                  Claims = new List<Claim>
                  {
                        new Claim(JwtClaimTypes.Name, "Mick Mining"),
                        new Claim("given_name", "Mick"),
                        new Claim("family_name", "Mining"),
                        new Claim("role", "Admin"),
                        new Claim("role", "Manager"),
                        new Claim("position", "Administrator"),
                        new Claim("country", "USA")
                  }
              },
              new TestUser
              {
                  SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
                  Username = "Jane",
                  Password = "JanePassword",
                  Claims = new List<Claim>
                  {
                        new Claim(JwtClaimTypes.Name, "Jane Downing"),
                        new Claim("given_name", "Jane"),
                        new Claim("family_name", "Downing"),
                        new Claim("role", "Visitor"),
                        new Claim("position", "Viewer"),
                        new Claim("country", "USA")
                  }
              }
          };



        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
               new Client
               {
                    ClientId = "user-store",
                    ClientSecrets = new [] { new Secret("dynamiIdentity4UserAPIDemo".Sha512()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "userApi" }
                },
               new Client
               {
                    ClientId = "product-store",
                    ClientSecrets = new [] { new Secret("dynamiIdentity4ProductAPIDemo".Sha512()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "productApi" }
                },
                new Client
                {
                    ClientName = "MVC Client",
                    ClientId = "mvc-client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>{ "https://localhost:5010/signin-oidc" },
                    RequirePkce = false,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile },
                    ClientSecrets = { new Secret("MVCSecret".Sha512()) }
                },
                new Client
                {
                    ClientId = "blazorWASM",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = { "https://localhost:5020" },
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "productApi",
                        "userApi",
                        IdentityServerConstants.LocalApi.ScopeName
                    },
                    RedirectUris = { "https://localhost:5020/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:5020/authentication/logout-callback" }
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope("productApi"),
                new ApiScope("userApi"),
                //new ApiScope("productApi", "Product Store API"),
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("productApi")
                {
                    Scopes = new List<string>{ "productApi" }
                },
                new ApiResource("userApi")
                {
                    Scopes = new List<string>{ "userApi" }
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
            };
    }
}
