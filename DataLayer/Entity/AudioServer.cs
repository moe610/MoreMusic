using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreMusic.DataLayer.Entity
{
    [Table("audio_server")]
    public class AudioServer
    {
        [Key]
        [Column("server_id")]
        public int Id { get; set; }

        [Column("server_path")]
        public string serverPath { get; set; }
    }
}
