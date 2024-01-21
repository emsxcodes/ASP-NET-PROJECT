using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bug.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "bit")]
        public bool IsAdmin { get; set; }
    }
}
