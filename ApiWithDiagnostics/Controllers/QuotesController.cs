using ApiWithDiagnostics.DbAccess;
using ApiWithDiagnostics.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithDiagnostics.Controllers;

[Route("[controller]")]
[ApiController]
public class QuotesController : ControllerBase
{
    private readonly IDbAccess _db;

    public QuotesController(IDbAccess db)
    {
        _db = db;
    }

    [HttpGet("random")]
    public async Task<IActionResult> RandomQuote()
    {
        Quote result = await _db.GetRandomQuote();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> NewQuote([FromQuery] QuoteDto quote)
    {
        await _db.NewQuote(quote);
        return Ok();
    }
}
