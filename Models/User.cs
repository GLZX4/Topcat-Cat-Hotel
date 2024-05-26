using System;
using Topcat_Cat_Hotel.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Topcat_Cat_Hotel.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public Role UserRole
        {
            get => Enum.Parse<Role>(Role);
            set => Role = value.ToString();
        }
    }
}
