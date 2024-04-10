

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDMVC.Models;

public partial class TicketLog
{
    public int Id { get; set; }

    [Display(Name = "Ticket")]
    public int TicketId { get; set; }

    [Display(Name = "Fecha de moficación")]
    public DateTime ModificationDate { get; set; }
    
    [Display(Name = "Comentario")]
    public string Comment { get; set; }

    public int UserId { get; set; }

    public Ticket Ticket { get; set; }
    // public  User User { get; set; }

}