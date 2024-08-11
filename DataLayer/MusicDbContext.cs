using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoreMusic.DataLayer.Entity;

namespace MoreMusic.DataLayer
{
    public class MusicDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<AudioFiles> AudioFiles { get; set; }
        public DbSet<ApplicationUser> SystemUsers { get; set; }
        public DbSet<AudioServer> AudioServer { get; set; }

        private readonly ILogger<MusicDbContext> _logger;

        public MusicDbContext(DbContextOptions<MusicDbContext> options, ILogger<MusicDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _logger.LogInformation("Configuring DbContext...");

            // ... other configuration ...

            base.OnConfiguring(optionsBuilder);
        }
    }
}
