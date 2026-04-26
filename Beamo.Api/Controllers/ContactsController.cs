using Beamo.Core.Models;
using Beamo.Core.Services.Database;
using Microsoft.AspNetCore.Mvc;

namespace Beamo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContactsController(IDatabaseService db) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetContacts(string userId)
        => Ok(await db.GetContactsAsync(userId));

    [HttpGet("{userId}/search")]
    public async Task<IActionResult> Search(string userId, [FromQuery] string q)
        => Ok(await db.SearchContactsAsync(userId, q));

    [HttpGet("{userId}/favorites")]
    public async Task<IActionResult> GetFavorites(string userId)
        => Ok(await db.GetFavoriteContactsAsync(userId));

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] Contact contact)
    {
        var result = await db.SaveAsync(contact);
        return result > 0 ? Ok(contact) : StatusCode(500);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await db.SoftDeleteAsync<Contact>(id);
        return result > 0 ? NoContent() : NotFound();
    }
}