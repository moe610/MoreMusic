using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreMusic.DataLayer.Entity
{
    [Table("system_users")]
    public class SystemUsers : IdentityUser
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_name")]
        public override string UserName { get; set; } // Use the overridden property from IdentityUser

        [Column("normalized_user_name")]
        public string NormalizedUserName { get; set; }

        [Column("email_address")]
        public override string Email { get; set; } // Use the overridden property from IdentityUser

        [Column("normalized_email_address")]
        public string NormalizedEmail { get; set; }

        [Column("email_confirmed")]
        public bool EmailConfirmed { get; set; }

        [Column("password_hash")]
        public override string PasswordHash { get; set; } // Use the overridden property from IdentityUser

        [Column("security_stamp")]
        public string SecurityStamp { get; set; }

        [Column("concurrency_stamp")]
        public string ConcurrencyStamp { get; set; }

        [Column("phone_number")]
        public override string PhoneNumber { get; set; } // Use the overridden property from IdentityUser

        [Column("phone_number_confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Column("two_factor_enabled")]
        public bool TwoFactorEnabled { get; set; }

        [Column("lockout_end")]
        public DateTimeOffset? LockoutEnd { get; set; }

        [Column("lockout_enabled")]
        public bool LockoutEnabled { get; set; }

        [Column("access_failed_count")]
        public int AccessFailedCount { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
