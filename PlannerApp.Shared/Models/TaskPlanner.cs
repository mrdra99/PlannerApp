using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Shared.Models
{
    public class TaskPlanner
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Category { get; set; } = string.Empty;
        public PriorityLevel Priority { get; set; }
        public bool IsCompleted { get; set; } = false;

        public bool EmptyVar { get; set; } = false;
    }

    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }
}
