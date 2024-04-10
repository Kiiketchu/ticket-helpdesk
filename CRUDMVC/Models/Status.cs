using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUDMVC.Models;

public partial class Status
{
    public int Id { get; set; }

    [Display(Name = "Status")]
    public string? Name { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
