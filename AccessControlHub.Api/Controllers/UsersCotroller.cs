using Microsoft.AspNetCore.Mvc;
using AccessControlHub.Domain.Entities;

namespace AccessControlHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static List<User> _users = new List<User>()
    {
        new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
        new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
    };

    [HttpGet]
    public ActionResult<List<User>> GetAll()
    {
        return Ok(_users);
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetById(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public ActionResult<User> Create(User newUser)
    {
        var newId = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
        newUser.Id = newId;

        _users.Add(newUser);

        return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id}")]
    public ActionResult<User> Update(int id, User updatedUser)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return NotFound();
        }

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return NotFound();
        }

        _users.Remove(user);

        return Ok();
    }
}