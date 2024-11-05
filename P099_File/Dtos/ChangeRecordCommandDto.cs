using P099_File.Models;

namespace P099_File.Dtos
{
    public class ChangeRecordCommandDto
    {
        public string EntityName { get; set; }
        public ChangeType ChangeType { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
