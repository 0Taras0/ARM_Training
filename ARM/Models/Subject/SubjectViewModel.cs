namespace ARM.Models.Subject
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public int TutorId { get; set; }
        public string TutorName { get; set; } = string.Empty;
        public List<int> Grades { get; set; } = new List<int>();
    }
}
