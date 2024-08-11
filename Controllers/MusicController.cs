using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoreMusic.DataLayer;
using MoreMusic.DataLayer.Entity;
using MoreMusic.Models;

namespace MoreMusic.Controllers
{
    [Authorize]
    public class MusicController : Controller
    {
        private readonly ILogger<MusicController> _logger;
        private readonly MusicDbContext _dbContext;
        private List<AudioFiles> _audioFiles;
        private int _currentIndex;


        public MusicController(ILogger<MusicController> logger, MusicDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _audioFiles = dbContext.audioFiles.ToList();
            _currentIndex = 0;

            //TODO
            //create functionality to play audio track alphabetically
            //create functionality to select shuffle mode to play audio tracks randomly
        }

        public IActionResult MusicPage()
        {
            _audioFiles = _audioFiles.Where(x => x.FileType == "aac").ToList();
            return View("MusicPage", new MusicViewModel { CurrentIndex = _currentIndex, AudioFiles = _audioFiles });
        }
    }
}
