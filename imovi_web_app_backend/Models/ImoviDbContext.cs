using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imovi_web_app_backend.Models {
    public class ImoviDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Models.Comment> Comments { get; set; }
        
        public ImoviDbContext(DbContextOptions<ImoviDbContext> options)
            : base(options) {
            Database.EnsureCreated();
        }
    }
}
