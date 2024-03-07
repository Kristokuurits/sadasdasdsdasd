﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITB2203Application.Model;

namespace ITB2203Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendeeController : ControllerBase
    {
        private readonly DataContext _context;

        public AttendeeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Attendee>> GetAttendee(string? name = null)
        {
            var query = _context.Attendee!.AsQueryable();

            if (name != null)
                query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

            return query.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Attendee> GetAttendee(int id)
        {
            var attendee = _context.Attendee!.Find(id);

            if (attendee == null)
            {
                return NotFound();
            }

            return attendee;
        }

        [HttpPut("{id}")]
        public IActionResult PutAttendee(int id, Attendee attendee)
        {
            var dbAttendee = _context.Attendee!.Find(id);
            if (id != attendee.Id || dbAttendee == null)
            {
                return NotFound();
            }

            _context.Update(attendee);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Attendee> PostAttendee(Attendee attendee)
        {
            var dbExercise = _context.Attendee!.Find(attendee.Id);
            if (dbExercise == null)
            {
                _context.Add(attendee);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetAttendee), new { Id = attendee.Id }, attendee);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAttendee(int id)
        {
            var attendee = _context.Attendee!.Find(id);
            if (attendee == null)
            {
                return NotFound();
            }

            _context.Remove(attendee);
            _context.SaveChanges();

            return NoContent();
        }
    }
}