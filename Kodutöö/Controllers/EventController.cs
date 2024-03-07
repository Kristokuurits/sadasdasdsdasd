﻿using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly DataContext _context;

    public EventController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Event>> GetEvent(string? name = null)
    {
        var query = _context.Event!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

        return query.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<TextReader> GetEvent(int id)
    {
        var e = _context.Event!.Find(id);

        if (e == null)
        {
            return NotFound();
        }

        return Ok(e);
    }

    [HttpPut("{id}")]
    public IActionResult PutEvent(int id, Event e)
    {
        var dbEvent = _context.Event!.AsNoTracking().FirstOrDefault(x => x.Id == e.Id);
        if (id != e.Id || dbEvent == null)
        {
            return NotFound();
        }

        _context.Update(e);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Event> PostEvent(Event e)
    {
        var dbExercise = _context.Event!.Find(e.Id);
        if (dbExercise == null)
        {
            _context.Add(e);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEvent), new { Id = e.Id }, e);
        }
        else
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEvent(int id)
    {
        var e = _context.Event!.Find(id);
        if (e == null)
        {
            return NotFound();
        }

        _context.Remove(e);
        _context.SaveChanges();

        return NoContent();
    }
}
