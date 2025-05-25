using ARM.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARM.Data.Entities
{
    [Table("tbl_subjects")]
    public class SubjectEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public GroupEntity? Group { get; set; }

        [ForeignKey("Tutor")]
        public int TutorId { get; set; }
        public TutorEntity Tutor { get; set; } = null!;
    }
}
