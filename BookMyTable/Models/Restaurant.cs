namespace BookMyTable.Models;

public class Restaurant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Cuisine { get; set; }
    public string Description { get; set; }
    public List<Table> Tables { get; set; } = new();
}
