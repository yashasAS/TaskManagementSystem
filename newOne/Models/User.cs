using System;
using System.Collections.Generic;

namespace newOne.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string? Role { get; set; }
        public string? Team { get; set; }
        public string Password { get; set; } = null!;
    }
}
