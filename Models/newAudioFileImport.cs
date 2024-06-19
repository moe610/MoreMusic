using System.ComponentModel.DataAnnotations.Schema;

namespace MoreMusic.Models
{
    public class newAudioFileImport
    {
        public string fileName { get; set; }
        public string filePath { get; set; }
        public string fileType { get; set; }
        public string title { get; set; }
        public int serverId { get; set; }

    }
}
