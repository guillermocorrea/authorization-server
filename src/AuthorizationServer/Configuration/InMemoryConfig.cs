using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AuthorizationServer.Configuration
{
  public static class InMemoryConfig
  {
    public static IEnumerable<IdentityResource> GetIdentityResources() =>
      new List<IdentityResource>
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Address(),
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
        new Claim("given_name", "Mick"),
        new Claim("family_name", "Mining"),
        new Claim("address", "Long Avenue 289"),
        new Claim("role", "Admin"),
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
        new Claim("given_name", "Jane"),
        new Claim("family_name", "Downing"),
        new Claim("address", "Sunny Street 4"),
        new Claim("role", "Visitor"),
        new Claim("position", "Viewer"),
        new Claim("country", "USA")
        }
        }
      };

    public static IEnumerable<ApiResource> GetApiResources() =>
      new List<ApiResource> { new ApiResource("jobBoardApi", "Job board API") };

    public static IEnumerable<Client> GetClients() =>
      new List<Client>
      {
        new Client
        {
        ClientId = "job-board",
        ClientSecrets = new [] { new Secret("Pa$$w0rd".Sha512()) },
        AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
        AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "jobBoardApi" }
        },
        new Client
        {
        ClientName = "MVC Client",
        ClientId = "mvc-client",
        AllowedGrantTypes = GrantTypes.Hybrid,
        RedirectUris = new List<string> { "https://localhost:5010/signin-oidc" },
        AllowedScopes = {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        IdentityServerConstants.StandardScopes.Address,
        "roles",
        "jobBoardApi",
        "position",
        "country"
        },
        ClientSecrets = { new Secret("MVCSecret".Sha512()) },
        PostLogoutRedirectUris = new List<string> { "https://localhost:5010/signout-callback-oidc" }
        }
      };
  }
}