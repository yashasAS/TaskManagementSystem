using System;
using System.Collections.Generic;

namespace newOne.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? AssignedTo { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
    }
}
