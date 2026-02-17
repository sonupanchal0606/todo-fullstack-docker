namespace TodoApi.Domain.Entities
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = "";
        public bool Done { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
