using Microsoft.AspNetCore.Mvc;
using MoreMusic.DataLayer;
using MoreMusic.Services;

namespace MoreMusic.Controllers
{
    public class UploadMusicController : Controller
    {
        private readonly ILogger<MusicController> _logger;
        private readonly MusicDbContext _dbContext;
        private readonly IConfiguration _configuration;


        public UploadMusicController(ILogger<MusicController> logger, MusicDbContext dbContext, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public IActionResult UploadMusicPage()
        {
            return View("UploadMusicPage");
        }

        // POST: /UploadMusic
        [HttpPost]
        public ActionResult Index(string youtubeUrl)
        {
            UploadMusicService uploadMusic = new UploadMusicService(_dbContext, _configuration);

            uploadMusic.Downloader(youtubeUrl);

            ViewBag.Message = "URL received: " + youtubeUrl;
            return View("UploadMusicPage");
        }

    }
}
