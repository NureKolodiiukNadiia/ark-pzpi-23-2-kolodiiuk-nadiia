using Microsoft.AspNetCore.Mvc;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;
using SpotRent.Services.Interfaces;
using SpotRent.Services.Spaces;

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
        [FromQuery] FilterCriterion[]? filterCriteria,
        [FromQuery] int? capacity,
        // [FromQuery] decimal?
        [FromQuery] int limit = 50,
        [FromQuery] int offset = 0)
    {
        // var queryResult = _spaceService.GetSpaces
        throw new NotImplementedException();
    }

    // GET /api/spaces/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSpace(int id)
    {
        var spaceResult = await _spaceService.GetSpaceByIdAsync(id);
        if (spaceResult.Failure)
        {
            return BadRequest(spaceResult.Error);
        }

        return Ok(spaceResult.Value);
    }

    // POST /api/spaces
    [HttpPost]
    public async Task<IActionResult> CreateSpace([FromBody] Space space)
    {
        var spaceCreationResult = await _spaceService.CreateSpaceAsync(space);
        if (spaceCreationResult.Failure)
        {
            return BadRequest(spaceCreationResult.Error);
        }

        return Ok(spaceCreationResult.Value);
    }

    // PUT /api/spaces/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSpace(int id, [FromBody] Space space)
    {
        var spaceUpdateResult = await _spaceService.UpdateSpaceAsync(space);
        if (spaceUpdateResult.Failure)
        {
            return BadRequest(spaceUpdateResult.Error);
        }

        return Ok(spaceUpdateResult.Value);
    }

    // DELETE /api/spaces/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSpace(int id)
    {
        var spaceDeletionResult = await _spaceService.DeleteSpaceAsync(id);
        if (spaceDeletionResult.Failure)
        {
            return BadRequest(spaceDeletionResult.Error);
        }

        return Ok();
    }

    // GET /api/spaces/{id}/schedule?startDate=2025-10-01&endDate=2025-10-31
    [HttpGet("{id:int}/schedule")]
    public async Task<ActionResult<SpaceSchedule>> GetSpaceSchedule(int id, [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        throw new NotImplementedException();
    }
}
