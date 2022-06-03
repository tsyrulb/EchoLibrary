using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class FeedbackContext : DbContext
    {
        public FeedbackContext (DbContextOptions<FeedbackContext> options)
            : base(options)
        {
        }

        public DbSet<Feedback> Feedback { get; set; }

    }
}
