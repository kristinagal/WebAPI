
namespace P099_File.Models
{
    public class ChangeRecord
    {
        public long Id { get; set; }
        public string EntityName { get; set; }
        public ChangeType ChangeType { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangeTime { get; set; }
    }
}
