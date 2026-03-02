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
        var result = await _eventService.GetEventByIdAsync(id, _userService.GetUserId());
        if (result.Status)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
    [HttpGet(Name = "GetEvents")]
    public async Task<IActionResult> GetEvents([FromQuery] EventFormQuery query)
    {
        var result = await _eventService.GetEventsAsync(query, _userService.GetUserId());
        if (result.Status)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
    [HttpPost(Name = "CreateEvent")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO data)
    {
        var result = await _eventService.CreateEventAsync(data, _userService.GetUserId());
        if (result.Status)
        {
            return CreatedAtRoute(nameof(GetEvent), new { id = result.Data?.Id }, result);
        }
        return BadRequest(result);
    }
    [HttpPut("{id}", Name = "UpdateEvent")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] CreateEventDTO data)
    {
        var result = await _eventService.UpdateEventAsync(id, data, _userService.GetUserId());
        if (result.Status)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpDelete("{id}", Name = "DeleteEvent")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var result = await _eventService.DeleteEventAsync(id, _userService.GetUserId());

        if (result.Status)
        {
            return NoContent();
        }
        return NotFound(result);
    }
}