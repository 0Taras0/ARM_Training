using ARM.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARM.Data.Entities
{
    [Table("tbl_grades")]
    public class GradeEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, 100)]
        public int Mark { get; set; } = 1;
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public StudentEntity? Student { get; set; }
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public SubjectEntity? Subject { get; set; }
    }
}
