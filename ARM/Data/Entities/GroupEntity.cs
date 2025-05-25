using ARM.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARM.Data.Entities
{
    [Table("tbl_groups")]
    public class GroupEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public ICollection<StudentEntity> Students { get; set; } = new List<StudentEntity>();

        [ForeignKey("Curator")]
        public int CuratorId { get; set; }
        public TutorEntity Curator { get; set; } = null!;
    }
}
