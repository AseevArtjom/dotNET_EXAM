using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dotNET_EXAM.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

    }
}