using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp.Services
{
  public interface IJobBoardApiClient
  {
    HttpClient GetClient();
  }
}