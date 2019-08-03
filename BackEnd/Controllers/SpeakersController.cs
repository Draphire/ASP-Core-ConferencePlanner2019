using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;

namespace BackEnd.Controllers
{
    //Attributes for path, Named Speaker
    //Api Controller Tells that the purpose Only for API for Machines
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ControllerBase
    { 
        //only for this class and only be created inside the constructor here
        //use ApplicaationDbContext Object
        private readonly ApplicationDbContext _db;

        public SpeakersController(ApplicationDbContext db)
        {
            //dependency injection passing in the database connection for methods below to function, Saves a copy
            _db = db;
        }
        // put,get,delete http functions. Send button = Post, Get = retrieve
        // GET: api/Speakers
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Speaker>>> GetSpeakers()
        // {
        //     //get from controller a list of Speakers return as a list, Returns as an Collection ActionResult
        //     //Action results format as Web Readable, Task = do asyncly do in background comaback and return as Task Object do later
        //     // Readable Forward only, Enumerates = IEnumerable  
        //     //Angle Brackets = Generic Type collection of speakers inside of this 
            
        //     // return await _db.Speakers.ToListAsync();

        //     //get speaker , readonly request, include sessions speaker connected to, reu
        //      var speakers = await _db.Speakers.AsNoTracking()
        //                     .Include(s => s.SessionSpeakers)
        //                         .ThenInclude(ss => ss.Session)
        //                     .ToListAsync();
        //     return speakers;   
        // }
         [HttpGet]
        public async Task<ActionResult<List<ConferenceDTO.SpeakerResponse>>> GetSpeakers()
        {
            //turns speaker to a list of speaker responses using the MapSpeakerResponse Function
            var speakers = await _db.Speakers.AsNoTracking()
                                            .Include(s => s.SessionSpeakers)
                                                .ThenInclude(ss => ss.Session)
                                            .Select(s => s.MapSpeakerResponse())
                                            .ToListAsync();
            return speakers;
        }
        //Option to get by ID
        //Defintes Verb to identify to be turned into Parameters for Method
        // GET: api/Speakers/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<Speaker>> GetSpeaker(int id)
        // {
        //     var speaker = await _db.Speakers.FindAsync(id);

        //     if (speaker == null)
        //     {
        //         return NotFound();
        //     }

        //     return speaker;
        // }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ConferenceDTO.SpeakerResponse>> GetSpeaker(int id)
        {
            var speaker = await _db.Speakers.AsNoTracking()
                                            .Include(s => s.SessionSpeakers)
                                                .ThenInclude(ss => ss.Session)
                                            .SingleOrDefaultAsync(s => s.ID == id);
            if (speaker == null)
            {
                return NotFound();
            }
            var result = speaker.MapSpeakerResponse();
            return result;
        }

        // //put = no update/return
        // //Id part of the Payload
        //  // PUT: api/Speakers/5
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutSpeaker(int id, Speaker speaker)
        // {


        //     if (id != speaker.ID)
        //     {
        //         return BadRequest();
        //     }

        //     _db.Entry(speaker).State = EntityState.Modified;

        //     try
        //     {
        //         await _db.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!SpeakerExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // // POST: api/Speakers
        // [HttpPost]
        // public async Task<ActionResult<Speaker>> PostSpeaker(Speaker speaker)
        // {
        //     _db.Speakers.Add(speaker);
        //     await _db.SaveChangesAsync();

        //     return CreatedAtAction("GetSpeaker", new { id = speaker.ID }, speaker);
        // }

        // // Takes into account the delete Verb if no attribute
        // // DELETE: api/Speakers/5
        // [HttpDelete("{id}")]
        // public async Task<ActionResult<Speaker>> DeleteSpeaker(int id)
        // {
        //     var speaker = await _db.Speakers.FindAsync(id);
        //     if (speaker == null)
        //     {
        //         return NotFound();
        //     }

        //     _db.Speakers.Remove(speaker);
        //     await _db.SaveChangesAsync();
        //     //Action Results formats it to be appropriate for web handling
        //     return speaker;
        // }

         [HttpPost]
        public async Task<ActionResult<ConferenceDTO.SpeakerResponse>> PostSpeaker(ConferenceDTO.Speaker input)
        {
            var speaker = new Speaker
            {
                Name = input.Name,
                WebSite = input.WebSite,
                Bio = input.Bio
            };

            _db.Speakers.Add(speaker);
            await _db.SaveChangesAsync();

            var result = speaker.MapSpeakerResponse();

            return CreatedAtAction(nameof(GetSpeaker), new { id = speaker.ID }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutSpeaker(int id, ConferenceDTO.Speaker input)
        {
            var speaker = await _db.FindAsync<Speaker>(id);

            if (speaker == null)
            {
                return NotFound();
            }

            speaker.Name = input.Name;
            speaker.WebSite = input.WebSite;
            speaker.Bio = input.Bio;

            // TODO: Handle exceptions, e.g. concurrency
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ConferenceDTO.SpeakerResponse>> DeleteSpeaker(int id)
        {
            var speaker = await _db.FindAsync<Speaker>(id);

            if (speaker == null)
            {
                return NotFound();
            }

            _db.Remove(speaker);
            await _db.SaveChangesAsync();

            return speaker.MapSpeakerResponse();
        }

        private bool SpeakerExists(int id)
        {
            return _db.Speakers.Any(e => e.ID == id);
        }
    }
}
