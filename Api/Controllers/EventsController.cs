using Application.DTOs.EventDTO;
using Application.Interfaces;
using Application.Models.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IUserService _userService;
    public EventsController(IEventService eventService, IUserService userService)
    {
        _eventService = eventService;
        _userService = userService;
    }

    [HttpGet("{id}",Name = "GetEvent")]
    public async Task<IActionResult> GetEvent(int id)
    {
        var result = await _eventService.GetEventByIdAsync(id);
        if (result.Status)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
    [HttpGet(Name = "GetEvents")]
    public async Task<IActionResult> GetEvents([FromQuery] EventFormQuery query)
    {
        var result = await _eventService.GetEventsAsync(query);
        if (result.Status)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
    [HttpPost(Name = "CreateEvent")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO data)
    {
        var result = await _eventService.CreateEventAsync(data);
        if (result.Status)
        {
            return CreatedAtRoute(nameof(GetEvent), result.Data);
        }
        return BadRequest(result);
    }
    [HttpDelete(Name = "DeleteEvent")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var result = await _eventService.DeleteEventAsync(id);

        if (result.Status)
        {
            return NoContent();
        }
        return NotFound(result);
    }
}