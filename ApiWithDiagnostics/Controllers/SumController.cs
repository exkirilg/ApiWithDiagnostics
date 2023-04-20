using Microsoft.AspNetCore.Mvc;

namespace ApiWithDiagnostics.Controllers;

[Route("[controller]")]
[ApiController]
public class SumController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> SumInt([FromQuery] int[] val)
    {
        int result = await Task.Run(val.Sum);
        return Ok(result);
    }
}
