using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using API.Entities;
using API.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class UsersController : BaseAPIController
  {
    private readonly DataContext _context;
    public UsersController(DataContext context)
    {
      _context = context;

    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsersAsync() {
      return await _context.Users.ToListAsync();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id) {
      return await _context.Users.FindAsync(id);
    }
  }
}