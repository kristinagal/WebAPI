using System;
using System.ComponentModel.DataAnnotations;

namespace P099_File.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
    }
}
