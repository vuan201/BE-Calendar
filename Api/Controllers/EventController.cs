using Application.DTOs.EventDTO;
using Application.Interfaces;
using Application.Models;
using Application.Models.Queries;
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

    [HttpGet("{id}",Name = "GetEvent")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _eventService.GetEventByIdAsync(id);
        if (result.Status)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
    [HttpGet(Name = "GetEvents")]
    public async Task<IActionResult> Get([FromQuery] EventFormQuery query)
    {
        var result = await _eventService.GetEventsAsync(query);
        if (result.Status)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
    [HttpPost(Name = "CreateEvent")]
    public async Task<IActionResult> Post([FromBody] CreateEventDTO data)
    {
        var result = await _eventService.CreateEventAsync(data);
        if (result.Status)
        {
            return CreatedAtRoute(nameof(Get), result.Data);
        }
        return BadRequest(result);
    }
    [HttpDelete(Name = "DeleteEvent")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _eventService.DeleteEventAsync(id);

        if (result.Status)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}