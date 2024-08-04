using System;
using System.Collections.Generic;

namespace newOne.Models
{
    public partial class Document
    {
        public int DocumentId { get; set; }
        public int? TaskId { get; set; }
        public string? FilePath { get; set; }
    }
}
