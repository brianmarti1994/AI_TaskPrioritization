namespace Shared
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
