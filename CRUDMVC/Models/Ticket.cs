using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUDMVC.Models;

public partial class Ticket
{
    public Ticket()
    {
        TicketLogs = new HashSet<TicketLog>();
    }
    public int Id { get; set; }

    [Display(Name = "Título")]
    public string Title { get; set; }

    [Display(Name = "Descripcion")]
    public string Description { get; set; }

    [Display(Name = "Actualizado")]
    public DateTime? UpdatedAt { get; set; }

    [Display(Name = "Creado")]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Tipo")]
    [Required]
    public int KindId { get; set; }

    [Display(Name = "Usuario")]
    [Required]
    public int UserId { get; set; }

    [Display(Name = "Asignado")]
    public int? AsignedId { get; set; }

    [Display(Name = "Proyecto")]
    public int? ProjectId { get; set; }

    [Display(Name = "Categoria")]
    public int? CategoryId { get; set; }

    [Display(Name = "Prioridad")]
    [Required]
    public int PriorityId { get; set; }

    [Display(Name = "Status")]
    [Required]
    public int StatusId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Kind? Kind { get; set; } = null!;

    public virtual Priority? Priority { get; set; } = null!;

    public virtual Project? Project { get; set; }

    public virtual Status? Status { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
    public virtual ICollection<TicketLog> TicketLogs { get; set; }
}

