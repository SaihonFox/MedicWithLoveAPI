using System.ComponentModel.DataAnnotations;

using MedicWithLoveAPI.Hubs;
using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;
using MedicWithLoveAPI.ModelsDTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MedicWithLoveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(PgSQLContext context, IHubContext<UserHub, IUserHub> hubContext) : ControllerBase
{
	[HttpGet("allusers")]
	public async Task<ActionResult<List<User>>> GetAllUsers() => await context.Users.ToListAsync();

	[HttpGet("users")]
	public async Task<ActionResult<List<User>>> GetDoctors() => await context.Users.Where(u => u.Post == 2).ToListAsync();

	[HttpGet("login")]
	public async Task<ActionResult<User>> Login([FromQuery] string login, [FromQuery] string password)
	{
		var users = await context.Users.ToListAsync();
		var user = users.Find(p => string.Equals(p.Login, login, StringComparison.CurrentCultureIgnoreCase) && p.Password == password);
		if (user == null)
			return NotFound();

		if (user.Post == 2 && (user.IsBlocked ?? false))
			return BadRequest("Пользователь заблокирован");

		return Ok(user);
	}

	[HttpPost]
	public async Task<ActionResult<User>> Register([FromBody] UserDTO user)
	{
		ArgumentNullException.ThrowIfNull(user, nameof(user));

		user.Id = 0;

		var users = await context.Users.ToListAsync();
		if (users.Any(u =>
			 u.Login.Equals(user.Login, StringComparison.OrdinalIgnoreCase) ||
			(u.Phone?.Equals(user.Phone) ?? false) ||
			(u.Passport?.Equals(user.Passport) ?? false)
		))
			return BadRequest("Пользователь с такими данными уже есть");
		var patients = await context.Patients.ToListAsync();
		if (patients.Any(p =>
			p.Login.Equals(user.Login, StringComparison.OrdinalIgnoreCase) ||
			(p.Phone?.Equals(user.Phone) ?? false) ||
			(p.Passport?.Equals(user.Passport) ?? false)
		))
			return BadRequest("Пользователь с такими данными уже есть");

		return Ok(user);
	}

	[HttpPut("block")]
	public async Task<ActionResult<User>> ChangeBlockStatus([FromQuery, Range(0, int.MaxValue)] int userId, [FromQuery] bool isBlocked)
	{
		var user = await context.Users.FindAsync(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

		user.IsBlocked = isBlocked;

		context.Users.Update(user);
		await context.SaveChangesAsync();

		await hubContext.Clients.All.UserUpdated(user);

		return user;
	}

	/*[HttpGet]
	public async Task<ActionResult<IEnumerable<User>>> GetUsers()
	{
		return await _context.Users.ToListAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<User>> GetUser(int id)
	{
		var user = await _context.Users.FindAsync(id);

		if (user == null)
			return NotFound();

		return user;
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> PutUser(int id, [FromBody] UserDTO user)
	{
		if (id != user.Id)
		{
			return BadRequest();
		}

		_context.Entry(user).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!UserExists(id))
			{
				return NotFound();
			}
			else
			{
				throw;
			}
		}

		return NoContent();
	}

	[HttpPost]
	public async Task<ActionResult<User>> PostUser([FromBody] UserDTO user)
	{
		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		return CreatedAtAction("GetUser", new { id = user.Id }, user);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteUser(int id)
	{
		var user = await _context.Users.FindAsync(id);
		if (user == null)
		{
			return NotFound();
		}

		_context.Users.Remove(user);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool UserExists(int id)
	{
		return _context.Users.Any(e => e.Id == id);
	}*/
}