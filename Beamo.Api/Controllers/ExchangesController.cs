using Beamo.Core.Models;
using Beamo.Core.Services.Database;
using Microsoft.AspNetCore.Mvc;

namespace Beamo.Api.Controllers;

[ApiController]
[Route("beamo/v1/[controller]")]
[Produces("application/json")]
public class ExchangesController(IDatabaseService db) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetExchanges(string userId)
        => Ok(await db.GetExchangesAsync(userId));

    [HttpGet("contact/{contactId}")]
    public async Task<IActionResult> GetForContact(string contactId)
        => Ok(await db.GetExchangesForContactAsync(contactId));

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] Exchange exchange)
    {
        var result = await db.SaveAsync(exchange);
        return result > 0 ? Ok(exchange) : StatusCode(500);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await db.SoftDeleteAsync<Exchange>(id);
        return result > 0 ? NoContent() : NotFound();
    }
}