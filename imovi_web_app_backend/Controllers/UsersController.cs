using imovi_web_app_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace imovi_web_app_backend.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase {
        ImoviDbContext db;
        public UsersController(ImoviDbContext context) {
            db = context;
            if (!db.Users.Any()) {
                db.Users.Add(new User { Email = "user1@gmail.com", Password = "1111", Name = "Tom", RegistrationDate = DateTime.Now.Date} );
                db.Users.Add(new User { Email = "user2@gmail.com", Password = "1111", Name = "Alice", RegistrationDate = DateTime.Now.Date} );
                db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get() {
            return await db.Users.ToListAsync();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id) {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();
            return new ObjectResult(user);
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<User>> Post(User user) {
            if (user == null) {
                return BadRequest();
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        // PUT api/users/
        [HttpPut]
        public async Task<ActionResult<User>> Put(User user) {
            if (user == null) {
                return BadRequest();
            }
            if (!db.Users.Any(x => x.Id == user.Id)) {
                return NotFound();
            }

            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id) {
            User user = db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpGet]
        [Route("login")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<User>> Login([FromHeader(Name = "Authorization")] string authHeader)
        {
            string encodedEmailPassword = authHeader.Substring("Basic ".Length).Trim();
            string emailPassword = Encoding
                .GetEncoding("iso-8859-1")
                .GetString(Convert.FromBase64String(encodedEmailPassword));
            // Get email and password
            int seperatorIndex = emailPassword.IndexOf(':');
            string email = emailPassword.Substring(0, seperatorIndex);
            string password = emailPassword.Substring(seperatorIndex + 1);

                User user = await db.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

            if (user == null)
                return NotFound("User not found");

            await Authenticate(user.Email);
            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<User>> Register([Bind] User u)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Email == u.Email || x.Name == u.Name);

            if (user == null)
            {
                user = new User() { Email = u.Email, Password = u.Password, Name = u.Name, RegistrationDate = DateTime.Now.Date };
                db.Users.Add(user);
                await db.SaveChangesAsync();
                await Authenticate(user.Email);

                return Ok(user);
            }
            else
            {
                return BadRequest("User with such email/username is already registered!");
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

        [HttpGet]
        [Route("logout")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

    }
}
