using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using imovi_web_app_backend.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace imovi_web_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        ImoviDbContext db;
        public CommentsController(ImoviDbContext context)
        {
            db = context;
            if (!db.Comments.Any())
            {
                db.Comments.Add(new Comment(){ MovieId = 632357, UserId = 1, Date = DateTime.Now, Rating = 5, Text = "Amazing movie!"});
                db.Comments.Add(new Comment(){ MovieId = 632357, UserId = 2, Date = DateTime.Now, Rating = 4, Text = "Very interesting movie"});
                db.SaveChanges();
            }
        }
        // GET: api/comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> Get()
        {
            return await db.Comments.ToListAsync();
        }

        // GET api/comments/5
        [HttpGet ("{movieId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> Get(int movieId)
        {
            IEnumerable<Comment> comments = await db.Comments.Where((x) => x.MovieId == movieId).ToListAsync();
            if (!comments.Any())
                return NoContent();

            return new ObjectResult(comments);
        }

        // POST api/comments/>
        [HttpPost]
        public async Task<ActionResult<Comment>> Post(Comment comment)
        {
            if (comment == null)
                return BadRequest("Wrong comment");
            if (comment.Text.Length > 300)
                return BadRequest("Comment text is too long!");
            db.Comments.Add(comment);
            await db.SaveChangesAsync();

            return Ok(comment);
        }

        // PUT api/comments/
        [HttpPut]
        public async Task<ActionResult<Comment>> Put(Comment comment)
        {
            if (comment == null)
                return BadRequest("Wrong comment");
            if (!db.Comments.Any(x => x.Id == comment.Id))
                return NotFound();
            if (comment.Text.Length > 300)
                return BadRequest("Comment text is too long!");
            
            db.Update(comment);
            await db.SaveChangesAsync();

            return Ok(comment);
        }

        // DELETE api/comments/
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comment>> Delete(int id)
        {
            Comment comment = db.Comments.FirstOrDefault(x => x.Id == id);
            if (comment == null)
                return NotFound();

            db.Comments.Remove(comment);
            await db.SaveChangesAsync();

            return Ok(comment);
        }
    }
}
