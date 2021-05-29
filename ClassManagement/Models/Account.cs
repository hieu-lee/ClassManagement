using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Models
{
    public class Account : IComparable<Account>
    {
        [Key]
        [Required]
        [StringLength(15, ErrorMessage = "Username length must be between 6 and 15 characters", MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Password length must be between 6 and 15 characters", MinimumLength = 6)]
        public string Password { get; set; }

        public int CompareTo(Account other)
        {
            return Username.CompareTo(other.Username);
        }
        public override bool Equals(object obj)
        {
            var other = (Account)obj;
            return Username.Equals(other.Username);
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }
    }
}
