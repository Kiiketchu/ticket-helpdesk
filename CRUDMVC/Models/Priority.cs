using System;
using System.Collections.Generic;

namespace CRUDMVC.Models;

public partial class Priority
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
