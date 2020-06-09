using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp.Services
{
  public interface IJobBoardApiClient
  {
    Task<HttpClient> GetClient();
  }
}