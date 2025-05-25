using ARM.Areas.Tutor.Models.Subject;

namespace ARM.Areas.Tutor.Models.Student
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public List<int> Grades { get; set; } = new List<int>();
    }
}
