using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Xml;
using TheaterEventPlanning.Data;
using TheaterEventPlanning.Models;

namespace TheaterEventPlanning.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    
    public class EventController : ControllerBase
    {
        private readonly EventDbContext _context;
        

        public EventController(EventDbContext context) => _context = context;




        //Detecting actor conflicts
        private (List<Event> conflictingEvents, List<string> conflictingActors) Conflict_Detection(Event updatedEvent, Event event_to_skip)
        {
            List<Event> conflictingEvents = new List<Event>();
            List<string> conflictingActors = new List<string>();

            foreach (Event @event in _context.Events)
            {
                if (@event != event_to_skip && // Exclude the event being updated
                    updatedEvent.startDate < @event.endDate &&
                    updatedEvent.endDate > @event.startDate)
                {
                    // Check for actor name conflicts within overlapping date range
                    foreach (var actorName in updatedEvent.CastMembers.Select(cm => cm.actorName))
                    {
                        if (@event.CastMembers.Any(cm => cm.actorName == actorName))
                        {
                            conflictingEvents.Add(@event);
                            conflictingActors.Add(actorName);
                            
                        }
                    }
                }
            }

            return (conflictingEvents, conflictingActors);
        }



        private Event Modify_Event(ControlEvent input_event)
        {
            var @event = new Event
            {
                name = input_event.name,
                startDate = input_event.startDate,
                endDate = input_event.endDate,
                location = input_event.location,
            };


            if (input_event.CastMembers != null && input_event.CastMembers.Any())
            {


                @event.CastMembers = input_event.CastMembers.Select(cm => new CastMember
                {
                    actorName = cm.actorName,
                    role = cm.role
                }).ToList();

            }
            return @event;
        }

        private string Message(List<Event> conflictingEvents, List<string> conflictingActors)
        {
            var conflictMessage = $"Conflict: Scheduling conflict with other event(s) and actor(s).\n";
            conflictMessage += "Conflicting Events:\n";

            foreach (var _event in conflictingEvents)
            {
                conflictMessage += $"Event Name: {_event.name}, Start Date: {_event.startDate}, End Date: {_event.endDate}\n";
            }

            if (conflictingActors.Any())
            {
                conflictMessage += "Conflicting Actors:\n";
                conflictMessage += string.Join(", ", conflictingActors);
            }
            return conflictMessage;

        }



        List<Event> Storage = new List<Event>();
      

        //creating events
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ControlEvent input_event)
        {

            List<string> names = new List<string>();

            if (!ModelState.IsValid)
            {
                
                return BadRequest(ModelState);
            }

           Event @event = Modify_Event(input_event);


            // Check for conflicts and get conflicting events
            var (conflictingEvents, conflictingActors) = Conflict_Detection(@event, null);

            if (conflictingEvents.Any())
            {
                if (conflictingEvents.Any())
                {
                    var conflictMessage = Message(conflictingEvents, conflictingActors);
                    return Conflict(conflictMessage);
                }
            }



            //Saving Data
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();
            Storage.Add(@event);

            return CreatedAtAction(nameof(GetById), new { id = @event.EventId }, @event);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, ControlEvent control_event)
        {
            var existingEvent = await _context.Events.Include(e => e.CastMembers).FirstOrDefaultAsync(e => e.EventId == id);
            if (existingEvent == null)
            {
                return BadRequest();
            }

            // Create a new Event object with the updated data
            var updatedEvent = Modify_Event(control_event);
            updatedEvent.EventId = id;

            // Check for conflicts and get conflicting events and actors
            var (conflictingEvents, conflictingActors) = Conflict_Detection(updatedEvent, existingEvent);

            if (conflictingEvents.Any())
            {
                var conflictMessage = Message(conflictingEvents, conflictingActors);
                return Conflict(conflictMessage);
            }

            // Update the existing event with the new data
            existingEvent.name = updatedEvent.name;
            existingEvent.startDate = updatedEvent.startDate;
            existingEvent.endDate = updatedEvent.endDate;
            existingEvent.location = updatedEvent.location;

            // Clear existing cast members and add the updated cast members
            existingEvent.CastMembers.Clear();

            foreach (var new_Cast_Member in updatedEvent.CastMembers)
            {
                existingEvent.CastMembers.Add(new CastMember
                {
                    actorName = new_Cast_Member.actorName,
                    role = new_Cast_Member.role,
                });
            }

            // Saving Data
            await _context.SaveChangesAsync();

            return Ok();
        }

        //deleting events
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var event_to_delete = await _context.Events.FindAsync(id);
            if (event_to_delete == null) { return NotFound("Event with the specified ID does not exist."); }

            _context.Events.Remove(event_to_delete);
            Storage.Remove(event_to_delete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //get all events
        [HttpGet]
        public async Task<IEnumerable<PartialEvent>> Get()
        {
            var events = await _context.Events.ToListAsync();
            var partial_event = events.Select(e => new PartialEvent
            {
                EventId = e.EventId,
                name = e.name,
                startDate = e.startDate,
                endDate = e.endDate,
                location = e.location,
                
            });

            return partial_event;
        }


        //get event by id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var _event = await _context.Events
                .Include(e => e.CastMembers)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (_event == null)
            {
                return NotFound("Event with the specified ID does not exist.");
            }

            return Ok(new Event
            {
                EventId = _event.EventId,
                name = _event.name,
                startDate = _event.startDate,
                endDate = _event.endDate,
                location = _event.location,
                CastMembers = _event.CastMembers
            });
        }
       

    }
}
