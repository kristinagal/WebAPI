using System;
using System.ComponentModel.DataAnnotations;

namespace P099_File.Models
{
    public class ChangeRecord
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required string EntityName { get; set; } // Requires C# 11+ for the 'required' keyword

        [Required]
        public ChangeType ChangeType { get; set; }

        public string? OldValue { get; set; } // Nullable if it’s not required to have a value

        public string? NewValue { get; set; } // Nullable if it’s not required to have a value

        [Required]
        public DateTime ChangeTime { get; set; } = DateTime.Now; // Default value
    }
}
