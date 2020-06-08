using System;
using System.Threading.Tasks;
using ClientApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
  }
}