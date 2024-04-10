using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDMVC.Models;

public partial class Project
{
    public int Id { get; set; }
    
    [Display(Name = "Nombre")]
    public string? Name { get; set; }

    [Display(Name = "Descripción")]
    public string? Description { get; set; }

    [Display(Name = "Contacto")]
    public string? Contact { get; set; }

    [Display(Name = "Comentarios")]
    public string? Comments { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
