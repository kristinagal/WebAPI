namespace P099_DTO.Dto
{
    public class TodoItemRequest
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
    }
}