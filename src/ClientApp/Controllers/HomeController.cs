using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ClientApp.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

namespace ClientApp.Controllers
{
  [Authorize]
  public class HomeController : Controller
  {
    private readonly IJobBoardApiClient _jobBoardApiClient;

    public HomeController(IJobBoardApiClient jobBoardApiClient)
    {
      _jobBoardApiClient = jobBoardApiClient;
    }
    public async Task<IActionResult> Index()
    {
      var client = _jobBoardApiClient.GetClient();
      var response = await client.GetAsync("api/jobs").ConfigureAwait(false);

      if (response.IsSuccessStatusCode)
      {
        var responseString = await response.Content.ReadAsStringAsync();
        var jobs = JsonConvert.DeserializeObject<string[]>(responseString);
        return View(jobs);
      }

      throw new Exception($"Problem with fetching data from the API: {response.ReasonPhrase}");
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Privacy()
    {
      var client = new HttpClient();
      var metaDataResponse = await client.GetDiscoveryDocumentAsync("https://localhost:5005");

      var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

      var response = await client.GetUserInfoAsync(new UserInfoRequest
      {
        Address = metaDataResponse.UserInfoEndpoint,
          Token = accessToken
      });

      if (response.IsError)
      {
        throw new Exception("Problem while fetching data from the UserInfo endpoint", response.Exception);
      }

      var addressClaim = response.Claims.FirstOrDefault(c => c.Type.Equals("address"));

      User.AddIdentity(new ClaimsIdentity(new List<Claim> { new Claim(addressClaim.Type.ToString(), addressClaim.Value.ToString()) }));

      return View();
    }
  }
}