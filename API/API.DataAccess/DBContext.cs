using API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DataAccess
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Video>? Videos { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Like>? Likes { get; set; }
        public DbSet<Dislike>? Dislikes { get; set; }
        public DbSet<Comment>? Comments { get; set; }
    }
}
