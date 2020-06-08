using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ClientApp.Services
{
  public class JobBoardApiClient : IJobBoardApiClient
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private HttpClient _httpClient;

    public JobBoardApiClient(IHttpContextAccessor httpContextAccessor)
    {
      _httpClient = new HttpClient();
      _httpContextAccessor = httpContextAccessor;
    }

    public HttpClient GetClient()
    {
      _httpClient.BaseAddress = new Uri("https://localhost:5001/");
      _httpClient.DefaultRequestHeaders.Accept.Clear();
      _httpClient.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

      return _httpClient;
    }
  }
}