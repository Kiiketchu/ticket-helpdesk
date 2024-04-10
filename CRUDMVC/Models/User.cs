using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.AspNetCore.Identity;


namespace CRUDMVC.Models;

public partial class User
{
    public User()
    {
        TicketLogs = new HashSet<TicketLog>();
    }
    public int Id { get; set; }

    [Display(Name = "Usuario")]
    public string Username { get; set; }

    [Display(Name = "Nombre")]
    public string Name { get; set; }

    [Display(Name = "Apellido")]
    public string Lastname { get; set; }

    [Display(Name = "Correo electrónico")]
    public string Email { get; set; }

    [Display(Name = "Contraseña")]
    public string Password { get; set; }
    public bool IsActive { get; set; }

    public int? Kind { get; set; }

    [Display(Name = "Creado")]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Rol")]
    public int? RoleId { get; set; }
    
    [Display(Name = "Proyecto")]
    public int? ProjectId { get; set; }    

    public virtual Project? Project { get; set; } = null!;
    public virtual Role? Role { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<TicketLog> TicketLogs { get; set; }
}
