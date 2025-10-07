using Microsoft.AspNetCore.Mvc;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;
using SpotRent.Services.Interfaces;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpacesController : ControllerBase
{
    private readonly ISpaceService _spaceService;

    public SpacesController(ISpaceService spaceService)
    {
        _spaceService = spaceService;
    }

    // GET /api/spaces?type=MeetingRoom&capacity=8&hasProjector=true&hasWiFi=true&isAvailable=true&limit=50&offset=0
    [HttpGet]
    public async Task<IActionResult> GetSpaces(
        [FromQuery] SpaceType? type,
        [FromQuery] int? capacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? hasWiFi,
        [FromQuery] bool? isAvailable,
        [FromQuery] int limit = 50,
        [FromQuery] int offset = 0)
    {
        throw new NotImplementedException();
    }

    // GET /api/spaces/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSpace(int id)
    {
        throw new NotImplementedException();
    }

    // POST /api/spaces
    [HttpPost]
    public async Task<IActionResult> CreateSpace([FromBody] Space space)
    {
        throw new NotImplementedException();
    }

    // PUT /api/spaces/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSpace(int id, [FromBody] Space space)
    {

        throw new NotImplementedException();
    }

    // DELETE /api/spaces/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSpace(int id)
    {
        throw new NotImplementedException();
    }

    // GET /api/spaces/{id}/schedule?startDate=2025-10-01&endDate=2025-10-31
    [HttpGet("{id:int}/schedule")]
    public async Task<IActionResult> GetSpaceSchedule(int id, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        throw new NotImplementedException();
    }
}
