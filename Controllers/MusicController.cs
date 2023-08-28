using Microsoft.AspNetCore.Mvc;
using MoreMusic.DataLayer;
using MoreMusic.Models;
using Newtonsoft.Json;

namespace MoreMusic.Controllers
{
    public class MusicController : Controller
    {
        private readonly ILogger<MusicController> _logger;
        private readonly MusicDbContext _dbContext;
        private List<AudioFiles> _audioFiles;
        private int _currentIndex;
        private int _previousIndex = -1;


        public MusicController(ILogger<MusicController> logger, MusicDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

            // Initialize _audioFiles with your list of filenames from the database
            _audioFiles = dbContext.audioFiles.ToList();

            // Initialize _currentIndex to 0
            _currentIndex = 0;
        }

        public IActionResult MusicPage()
        {
            try
            {
                _audioFiles = _audioFiles.Where(x => x.FileType == "mp3").ToList();
                return View("MusicPage", new MusicViewModel { CurrentIndex = _currentIndex, AudioFiles = _audioFiles });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IActionResult IphoneMusicPage()
        {
            _audioFiles = _audioFiles.Where(x => x.FileType == "aac").ToList();
            return View("IphoneMusicPage", new MusicViewModel { CurrentIndex = _currentIndex, AudioFiles = _audioFiles });
        }

        
        [HttpGet]
        public IActionResult GetNextFileName([FromQuery(Name = "audioFiles")] string serializedAudioFiles)
        {
            // Deserialize the JSON data back to your model
            List<AudioFiles> audioFiles = JsonConvert.DeserializeObject<List<AudioFiles>>(serializedAudioFiles);

            // Declare randomIndex here
            int randomIndex;

            // Generate a random index that is different from the previous one
            Random random = new Random();

            do
            {
                randomIndex = random.Next(0, audioFiles.Count);
            } while (randomIndex == _previousIndex);

            // Update _currentIndex and _previousIndex
            _previousIndex = _currentIndex;
            _currentIndex = randomIndex;

            // Return the next filename
            return Content(audioFiles[randomIndex].FileName);
        }
    }
}
