using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bug.Models
{
    public class BugReport
    {
        [Key]
        [Column(TypeName = "int")]
        public int ReportId { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Report { get; set; }

        [Column(TypeName = "int")]
        public int BugId { get; set; }

        public Bugs Bug { get; set; }
    }
}
