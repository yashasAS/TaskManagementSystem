using System;
using System.Collections.Generic;

namespace newOne.Models
{
    public partial class Note
    {
        public int NoteId { get; set; }
        public int TaskId { get; set; }
        public string? Content { get; set; }
    }
}
