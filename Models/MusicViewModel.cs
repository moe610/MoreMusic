using MoreMusic.DataLayer.Entity;

namespace MoreMusic.Models
{
    public class MusicViewModel
    {
        public int CurrentIndex { get; set; }
        public List<AudioFiles> AudioFiles { get; set; }
    }
}