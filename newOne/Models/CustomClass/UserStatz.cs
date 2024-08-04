namespace newOne.Models.CustomClass
{
    public class UserStatz
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string TeamName { get; set; }
        public List<Task> CompletedTasks { get; set; }
        public int CompletedTaskCount { get; set; }
        public List<Task> IncompletedTasks { get; set; }
        public int IncompletedTaskCount { get; set; }
        public int TotalTask { get; set; }
        public DateTime Time { get; set; }

    }
}
