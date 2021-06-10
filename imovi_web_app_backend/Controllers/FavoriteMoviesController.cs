using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using imovi_web_app_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace imovi_web_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteMoviesController : ControllerBase
    {
        ImoviDbContext db;
        public FavoriteMoviesController(ImoviDbContext context)
        {
            db = context;
            if (!db.FavoriteMovies.Any())
            {
                db.FavoriteMovies.Add(new FavoriteMovie() { MovieId = 652357, UserId = 1 });
                db.FavoriteMovies.Add(new FavoriteMovie() { MovieId = 399566, UserId = 1 });
                db.FavoriteMovies.Add(new FavoriteMovie() { MovieId = 652357, UserId = 2 });
                db.SaveChanges();
            }
        }

        // GET: api/favoritemovies/
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FavoriteMovie>>> Get()
        {
            return await db.FavoriteMovies.ToListAsync();
        }

        // GET api/favoritemovies/5
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<ActionResult<FavoriteMovie>> Get(int userId)
        {
            IEnumerable<FavoriteMovie> movies = await db.FavoriteMovies.Where(x => x.UserId == userId).ToListAsync();
            if (!movies.Any())
                return NotFound();

            return new ObjectResult(movies);
        }

        // POST api/favoritemovies
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> Post(FavoriteMovie movie)
        {
            if (movie == null)
                return BadRequest();
            if (!db.Users.Any(x => x.Id == movie.UserId))
                return BadRequest("There is no such user! (Wrong UserId)");
            if (db.FavoriteMovies.Any(x => x.MovieId == movie.MovieId && x.UserId == movie.UserId))
                return Ok(db.FavoriteMovies.FirstOrDefault(x => x.MovieId == movie.MovieId && x.UserId == movie.UserId));

            db.FavoriteMovies.Add(movie);
            await db.SaveChangesAsync();

            return Ok(movie);
        }

        // PUT api/favoritemovies
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<FavoriteMovie>> Put(FavoriteMovie movie)
        {
            if (movie == null)
                return BadRequest();
            if (db.Users.Any(x => x.Id == movie.UserId))
                return BadRequest("There is no such user! (Wrong UserId)");
            if (!db.FavoriteMovies.Any(x => x.Id == movie.Id))
                return NotFound();

            db.Update(movie);
            await db.SaveChangesAsync();

            return Ok(movie);
        }

        // DELETE api/favoritemovies
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<FavoriteMovie>> Delete(FavoriteMovie m)
        {
            FavoriteMovie movie = db.FavoriteMovies.FirstOrDefault(x => x.UserId == m.UserId && x.MovieId == m.MovieId);
            if (movie == null)
                return NotFound();

            db.FavoriteMovies.Remove(movie);
            await db.SaveChangesAsync();

            return Ok(movie);
        }
    }
}
