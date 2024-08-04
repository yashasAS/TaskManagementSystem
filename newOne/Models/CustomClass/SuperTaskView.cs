using DbTask = newOne.Models.Task;

namespace newOne.Models.CustomClass
{
    public class SuperTaskView
    {
        public int TaskId { get; set; }
        public int AssignedTo { get; set; }
        public DbTask Task { get; set; }
        public List<Note> Notes { get; set; }
        public List<Document> Documents { get; set; }
    }
}
