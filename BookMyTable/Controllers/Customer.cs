using BookMyTable.Data;
using BookMyTable.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace BookMyTable.Controllers;
[Route("api/[controller]")]
[ApiController]
public class Customer(AppDbContext context) : ControllerBase
{
    [HttpGet("restaurants")]
    public async Task<ActionResult<Restaurant>> RestaurantListing()
    {
        List<Restaurant> list =await context.Restaurants.ToListAsync();
        if (list == null)
            return BadRequest("No Restaurants Found.");
        return Ok(list);
    }

    [HttpGet("restaurants/{restaurantId}/tables")]
    public async Task<ActionResult<Restaurant>> TableAvailability(Guid restaurantId)
    {
        var tables = await context.Tables.Where(t => t.RestaurantId == restaurantId).ToListAsync();

        if (!tables.Any())
            return NotFound("No tables found for this restaurant.");

        return Ok(tables);
    }

    [Authorize]
    [HttpPost("reservations")]
    public async Task<ActionResult<Restaurant>> makeReservation(Guid tableId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized("User ID not found in token.");
        Guid userId = Guid.Parse(userIdClaim.Value);

        var table = await context.Tables.FindAsync(tableId);
        if (table == null)
            return NotFound("Table not found.");

        var existingReservations = await context.Reservations.CountAsync(r => r.TableId == tableId);
        if (existingReservations >= table.Capacity)
            return BadRequest("No available seats at this table.");

        User? user = await context.Users.FirstOrDefaultAsync(u => u.Id==userId);

        Reservation reservation = new Reservation
        {
            ReservationId = Guid.NewGuid(),
            ReservationDateTime = DateTime.UtcNow,
            Status = "Booked",
            UserId = userId,
            TableId = table.Id
        };
        await context.Reservations.AddAsync(reservation);
        await context.SaveChangesAsync();
        return Ok("Reservation Done.\nReservation Id: "+reservation.ReservationId);
    }

    [Authorize]
    [HttpGet("reservations/{reservationId}")]
    public async Task<ActionResult<Reservation>> viewReservation(Guid reservationId)
    {
        Reservation? reservation = await context.Reservations.Include(r => r.Table).FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        if (reservation is null) return BadRequest("No reservation found.");
        return Ok(reservation);
    }

    [Authorize]
    [HttpDelete("reservations/{reservationId}")]
    public async Task<ActionResult<Reservation>> cancelReservation(Guid reservationId)
    {
        Reservation? reservation = await context.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        if (reservation is null) return BadRequest("No reservation with this Id.");

        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync();

        return Ok("Reservation canceled.");
    }
}
