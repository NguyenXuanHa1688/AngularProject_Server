using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string UserName { get; set; } = string.Empty; 

        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

    }
}
