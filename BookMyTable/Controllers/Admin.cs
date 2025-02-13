using BookMyTable.Data;
using BookMyTable.DTOs;
using BookMyTable.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Table = BookMyTable.Models.Table; 
namespace BookMyTable.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Admin(AppDbContext context) : ControllerBase
{
    [Authorize(Roles ="Admin")]
    [HttpPost("admin/restaurants")]
    public async Task<ActionResult<Restaurant>> createRestaurant([FromBody] RestaurantDTO dto)
    {
        Restaurant restaurant = new Restaurant
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Location = dto.Location,
            Cuisine = dto.Cuisine,
            Description = dto.Description,
        };
        await context.Restaurants.AddAsync(restaurant);
        await context.SaveChangesAsync();
        return Ok("Restaurant Created.");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("restaurants")]
    public async Task<ActionResult<Restaurant>> viewRestaurants()
    {
        List<Restaurant>? restaurants = await context.Restaurants.ToListAsync();
        if (!restaurants.Any()) return BadRequest("No restaurants found.");
        return Ok(restaurants);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("restaurants/{id}")]
    public async Task<ActionResult<Restaurant>> viewRestaurant(Guid id)
    {
        Restaurant? restaurant = await context.Restaurants.FirstOrDefaultAsync(r=>r.Id==id);
        if (restaurant == null) return BadRequest("No restaurant found.");
        return Ok(restaurant);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("restaurants/{id}")]
    public async Task<ActionResult<Restaurant>> updateRestaurant(Guid id,string restaurantName, string restaurantLocation, string restaurantCuisine, string restaurantDescription)
    {

        Restaurant? restaurant = await context.Restaurants.FirstOrDefaultAsync(r => r.Id==id);
        if (restaurant == null) return BadRequest("No restaurant found.");

        restaurant.Name = restaurantName;
        restaurant.Location = restaurantLocation;
        restaurant.Cuisine = restaurantCuisine;
        restaurant.Description = restaurantDescription;

        context.Restaurants.Update(restaurant);
        await context.SaveChangesAsync();
        return Ok("Restaurant Updated.");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("restaurants/{id}")]
    public async Task<ActionResult<Restaurant>> deleteRestaurant(Guid id)
    {
        Restaurant? restaurant = await context.Restaurants.FirstOrDefaultAsync(r => r.Id==id);
        if (restaurant == null) return BadRequest("No restaurant found.");

        context.Restaurants.Remove(restaurant);
        await context.SaveChangesAsync();

        return Ok("Restaurant Removed.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("tables")]
    public async Task<ActionResult<Table>> createTables(int tableNumber, int capacity, Guid restaurantId)
    {
        Restaurant? restaurant = await context.Restaurants.FirstOrDefaultAsync(r=>r.Id==restaurantId);
        if (restaurant == null) return BadRequest("No Restaurant found.");

        Table table = new Table
        {
                Id = Guid.NewGuid(),
                TableNumber =tableNumber,
                Capacity = capacity,
                RestaurantId = restaurantId
        };
        await context.Tables.AddAsync(table);
        await context.SaveChangesAsync();
        return Ok("Table Created.");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("tables")]
    public async Task<ActionResult<Table>> viewTables()
    {
        List<Table>? tables = await context.Tables.ToListAsync();
        if (!tables.Any()) return BadRequest("No Tables found.");
        return Ok(tables);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("tables/{id}")]
    public async Task<ActionResult<Table>> viewTable(Guid id)
    {
        Table? Table = await context.Tables.FirstOrDefaultAsync(t => t.Id==id);
        if (Table == null) return BadRequest("No Table found.");
        return Ok(Table);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("tables/{id}")]
    public async Task<ActionResult<Table>> updateTable(Guid id, int tableNumber, int capacity, Guid restaurantId)
    {

        Table? table = await context.Tables.FirstOrDefaultAsync(r => r.Id==id);
        if (table == null) return BadRequest("No Table found.");

        table.TableNumber = tableNumber;
        table.Capacity = capacity;
        table.RestaurantId = restaurantId;

        context.Tables.Update(table);
        await context.SaveChangesAsync();
        return Ok("Table Updated.");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("tables/{id}")]
    public async Task<ActionResult<Table>> deleteTable(Guid id)
    {
        Table? table = await context.Tables.FirstOrDefaultAsync(r => r.Id==id);
        if (table == null) return BadRequest("No table found.");

        context.Tables.Remove(table);
        await context.SaveChangesAsync();

        return Ok("table Removed.");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("reservations")]
    public async Task<ActionResult<Reservation>> viewReservations()
    {
        List<Reservation>? reservations = await context.Reservations.ToListAsync();
        if (!reservations.Any()) return BadRequest("No reservations found.");
        return Ok(reservations);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("reservations/{id}")]
    public async Task<ActionResult<Reservation>> viewReservation(Guid id)
    {
        Reservation? reservation = await context.Reservations.FirstOrDefaultAsync(r => r.ReservationId==id);
        if (reservation == null) return BadRequest("No reservations found.");
        return Ok(reservation);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("reservations/{id}")]
    public async Task<ActionResult<Reservation>> deleteReservation(Guid id)
    {
        Reservation? reservation = await context.Reservations.FirstOrDefaultAsync(r => r.ReservationId==id);
        if (reservation == null) return BadRequest("No reservations found.");

        var table = await context.Tables.FirstOrDefaultAsync(t => t.Id == reservation.TableId);
        if (table != null) table.Capacity++;

        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync();

        return Ok("Reservation Removed.");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("reservations/stats")]
    public async Task<ActionResult> GetReservationStats()
    {
        var totalReservations = await context.Reservations.CountAsync();
        var mostBookedRestaurant = await context.Reservations
            .GroupBy(r => r.Table.RestaurantId)
            .OrderByDescending(g => g.Count())
            .Select(g => new { RestaurantId = g.Key, Count = g.Count() })
            .FirstOrDefaultAsync();

        return Ok(new
        {
            TotalReservations = totalReservations,
            MostBookedRestaurant = mostBookedRestaurant
        });
    }

}
