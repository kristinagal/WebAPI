using P099_File.Models;

namespace P099_File.Dtos
{
    public class ChangeRecordDto
    {
        public long Id { get; set; }
        public required string EntityName { get; set; } 
        public ChangeType ChangeType { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangeTime { get; set; }
    }
}
