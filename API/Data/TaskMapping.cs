using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data
{
    using Shared;

    public static class TaskMapping
    {
        public static TaskItemDto ToDto(TaskItem e) => new()
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            DueDate = e.DueDate,
            Priority = ParsePriority(e.Priority)
        };

        private static TaskPriority ParsePriority(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return TaskPriority.Medium;
            var t = s.Trim().ToLowerInvariant();
            return t switch
            {
                "high" => TaskPriority.High,
                "medium" or "mid" => TaskPriority.Medium,
                "low" => TaskPriority.Low,
                _ => TaskPriority.Medium
            };
        }
    }

}
