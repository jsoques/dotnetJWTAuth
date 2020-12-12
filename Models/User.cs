using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuth.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "text(256)")] //Sqlite using DA Schema
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "text(124)")] //Sqlite using DA Schema
        public string PasswordHash { get; set; }

        public UserStatus Status { get; set; }

        [Required]
        [Column(TypeName = "text(25)")] //Sqlite using DA Schema
        public string DateCreated { get; set; }

        [Required]
        [Column(TypeName = "text(32)")] //Sqlite using DA Schema
        public string ActivateKey { get; set; }

    }

    public enum UserStatus
    {
        Disabled = 0,
        Enabled = 1

    }
}