using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreMusic.DataLayer.Entity
{
    [Table("system_users")]
    public class SystemUsers
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_name")]
        public string UserName { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("email_address")]
        public string EmailAddress { get; set; }
        [Column("password")]
        public string Password { get; set; }
    }
}
