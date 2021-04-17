using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class BuggyController : BaseAPIController
  {
    private readonly DataContext _dataContext;
    public BuggyController(DataContext dataContext)
    {
      _dataContext = dataContext;
    }

    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
      return "secret text";
    }
    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
      var thing = _dataContext.Users.Find(-1);

      if (thing == null) return NotFound();

      return Ok(thing);
    }
    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
      var thing = _dataContext.Users.Find(-1);
      return thing.UserName;
    }
    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
      return BadRequest("This wasn't a good request.");
    }
  }
}