using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using imovi_web_app_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace imovi_web_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentReplyController : ControllerBase
    {
        ImoviDbContext db;
        public CommentReplyController(ImoviDbContext context)
        {
            db = context;
        }
        // GET: api/commentreply
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentReply>>> Get()
        {
            return await db.CommentReplies.ToListAsync();
        }

        // GET api/comments/5
        [HttpGet("{commentId}")]
        public async Task<ActionResult<IEnumerable<CommentReply>>> Get(int commentId)
        {
            IEnumerable<CommentReply> comments = await db.CommentReplies.Where((x) =>
            x.CommentId == commentId).ToListAsync();
            if (!comments.Any())
                return NoContent();

            return new ObjectResult(comments);
        }

        // POST api/comments/>
        [HttpPost]
        [Route("add")]
        //[Authorize]
        public async Task<ActionResult<Comment>> Post(CommentReply commentReply)
        {
            if (commentReply == null)
                return BadRequest("Wrong comment reply");

            User currUser = db.Users.FirstOrDefault(
                x => x.Email == User.Identity.Name);
            commentReply.UserId = currUser.Id;
            commentReply.Date = DateTime.Now;

            db.CommentReplies.Add(commentReply);
            await db.SaveChangesAsync();

            return Ok(commentReply);
        }

        // PUT api/comments/edit
        [HttpPut]
        [Route("edit")]
        [Authorize]
        public async Task<ActionResult<Comment>> Put(Comment comment)
        {
            return Ok(comment);
        }

        // POST api/comments/delete
        [HttpPost]
        [Route("delete")]
        [Authorize]
        public async Task<ActionResult<Comment>> Delete(Comment comment)
        {
            return Ok(comment);
        }
    }
}


