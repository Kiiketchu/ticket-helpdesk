using CRUDMVC.Models;

public class TicketEditViewModel
{
    public Ticket Ticket { get; set; }
    public List<TicketLog> Logs { get; set; }
}