using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bug.Models
{
    public class BugFix
    {
        [Key]
        [Column(TypeName = "int")]
        public int FixId { get; set; }
        
        [Required(ErrorMessage = "Description is required")]
        [Column(TypeName = "nvarchar(200)")]
        public string FixDescription { get; set; }

        [Column(TypeName = "int")]
        public int BugId { get; set; }

        public required Bugs Bug { get; set; }
    }
}
