using ARM.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ARM.Data.Entities
{
    [Table("tbl_tutors")]
    public class TutorEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserEntity? User { get; set; }
    }
}
