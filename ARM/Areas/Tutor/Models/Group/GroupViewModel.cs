using ARM.Areas.Tutor.Models.Student;

namespace ARM.Areas.Tutor.Models.Group
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<StudentViewModel> Students { get; set; } = new List<StudentViewModel>();
    }
}
