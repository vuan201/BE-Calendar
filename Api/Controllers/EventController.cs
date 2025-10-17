using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet(Name = "GetEvent")]
    public async Task<IActionResult> Get()
    {
        var result = await _eventService.GetEventsAsync();
        if (result != null)
        {
            return Ok(result);
        }
        return NotFound();
    }
}