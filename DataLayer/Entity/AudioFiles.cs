using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreMusic.DataLayer.Entity
{
    [Table("audio_files")]
    public class AudioFiles
    {
        [Key]
        [Column("file_id")]
        public int Id { get; set; }

        [Column("file_name")]
        public string FileName { get; set; }

        [Column("file_path")]
        public string FilePath { get; set; }

        [Column("file_type")]
        public string FileType { get; set; }

        [Column("title")]
        public string Title { get; set; }
        [Column("server_id")]
        public int ServerId { get; set; }

    }
}
