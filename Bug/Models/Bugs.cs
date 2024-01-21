using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bug.Models
{
    public class Bugs
    {
        [Key]
        [Column(TypeName = "int")]
        public int BugId { get; set; }
        
        [Required(ErrorMessage = "Description is required")]
        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(450)")] 
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }

}
