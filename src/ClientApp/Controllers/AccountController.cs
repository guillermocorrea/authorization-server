using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers
{
  public class AccountController : Controller
  {
    public async Task Logout()
    {
      await HttpContext.SignOutAsync("Cookies");
      await HttpContext.SignOutAsync("oidc");
    }
  }
}