namespace BookMyTable.Models;

public class Table
{
    public Guid Id { get; set; }
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public List<Reservation> Reservations { get; set; } = new();
}
