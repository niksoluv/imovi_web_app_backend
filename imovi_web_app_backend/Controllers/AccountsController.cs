using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using imovi_web_app_backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace imovi_web_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private ImoviDbContext db;

        public AccountsController(ImoviDbContext context)
        {
            db = context;
        }

        // GET: api/<AccountsController>
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<User>> Login(User u)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Email == u.Email && x.Password == u.Password);

            if (user == null)
                return NotFound("User not found");

            await Authenticate(user.Email);

            return Ok(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<User>> Register(User u)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Email == u.Email && x.Password == u.Password);

            if (user == null)
            {
                user = new User() {Email = user.Email, Password = user.Password};
                db.Users.Add(user);
                await db.SaveChangesAsync();
                await Authenticate(user.Email);

                return Ok(user);
            }
            else
            {
                return BadRequest("User is already registered!");
            }
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
