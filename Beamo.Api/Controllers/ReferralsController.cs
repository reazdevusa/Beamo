using Beamo.Core.Models;
using Beamo.Core.Services.Database;
using Microsoft.AspNetCore.Mvc;

namespace Beamo.Api.Controllers;

[ApiController]
[Route("beamo/v1/[controller]")]
[Produces("application/json")]
public class ReferralsController(IDatabaseService db) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetReferrals(string userId)
        => Ok(await db.GetReferralsAsync(userId));

    [HttpGet("{userId}/earnings")]
    public async Task<IActionResult> GetEarnings(string userId)
        => Ok(await db.GetEarningsAsync(userId));

    [HttpGet("{userId}/earnings/total")]
    public async Task<IActionResult> GetTotalEarnings(string userId)
        => Ok(new { userId, total = await db.GetTotalEarningsAsync(userId) });

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] Referral referral)
    {
        var result = await db.SaveAsync(referral);
        return result > 0 ? Ok(referral) : StatusCode(500);
    }
}