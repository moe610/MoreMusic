using Microsoft.EntityFrameworkCore;
using MoreMusic.DataLayer.Entity;

namespace MoreMusic.DataLayer
{
    public class MusicDbContext : DbContext
    {
        public DbSet<AudioFiles> audioFiles { get; set; }
        public DbSet<SystemUsers> systemUsers { get; set; }
        public DbSet<AudioServer> audioServer { get; set; }
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
