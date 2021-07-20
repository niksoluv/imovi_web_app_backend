using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using imovi_web_app_backend.Models;

namespace imovi_web_app_backend.Models {
    public class ImoviDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FavoriteMovie> FavoriteMovies { get; set; }
        public DbSet<CommentReply> CommentReplies { get; set; }

        public ImoviDbContext(DbContextOptions<ImoviDbContext> options)
            : base(options) {
            Database.EnsureCreated();
        }
    }
}
