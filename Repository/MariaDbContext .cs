using Microsoft.EntityFrameworkCore;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public partial class MariaDbContext : DbContext
    {
        public MariaDbContext(DbContextOptions<MariaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> UserDB { get; set; }

        public virtual DbSet<Contact> ContactDB { get; set; }

        public virtual DbSet<Message> MessageDB { get; set; }
     /*   protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasOne<User>(c => c.User)
                .WithMany(c => c.contacts)
                .HasForeignKey(c => c.id);
        }*/
    }
}
