using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardApi.Controllers
{
  [Route("api/jobs")]
  [ApiController]
  [Authorize]
  public class JobsController : ControllerBase
  {
    public IActionResult GetJobs()
    {
      return Ok(
        new string[] { ".NET Develop", "React Sr Dev", "Angular Sr Dev", "Full stack - Node/react" }
      );
    }
  }
}