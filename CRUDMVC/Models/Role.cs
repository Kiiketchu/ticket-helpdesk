using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDMVC.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Display(Name = "Rol")]
        public string? Name { get; set; }

        [NotMapped]
        public virtual User? User { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
