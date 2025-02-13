namespace BookMyTable.Models;

public class Reservation
{
    public Guid ReservationId { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public string Status { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid TableId { get; set; }
    public Table Table { get; set; }
}
