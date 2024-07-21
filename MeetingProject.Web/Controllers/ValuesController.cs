using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingProject.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok(new { message = "This is a protected endpoint" });
    }
}
