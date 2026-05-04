using Beamo.Core.Models;
using Beamo.Core.Services.Database;
using Microsoft.AspNetCore.Mvc;

namespace Beamo.Api.Controllers;

[ApiController]
[Route("beamo/v1/[controller]")]
[Produces("application/json")]
public class NotificationsController(IDatabaseService db) : ControllerBase
{
    [HttpGet("{userId}/unread")]
    public async Task<IActionResult> GetUnread(string userId)
        => Ok(await db.GetUnreadNotificationsAsync(userId));

    [HttpPatch("{id}/read")]
    public async Task<IActionResult> MarkRead(string id)
    {
        await db.MarkNotificationReadAsync(id);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] Notification notification)
    {
        var result = await db.SaveAsync(notification);
        return result > 0 ? Ok(notification) : StatusCode(500);
    }
}