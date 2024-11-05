using Microsoft.AspNetCore.Mvc;
using P099_File.Dtos;
using P099_File.Models;
using P099_File.Services;

namespace P099_File.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChangeTrackerController : ControllerBase
    {
        private readonly IDbChangeTracker _changeTracker;
        private readonly IChangeRecordMapper _mapper;

        public ChangeTrackerController(IDbChangeTracker changeTracker, IChangeRecordMapper mapper)
        {
            _changeTracker = changeTracker;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ChangeRecordDto>> GetAllChanges()
        // GET: grąžina visus stebimus pakeitimus (naudoti uzklausos DTO)
        {
            var changes = _changeTracker.GetChanges();
            if (changes == null || !changes.Any())
            {
                return NotFound("No changes found.");
            }

            var result = _mapper.Map(changes);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<ChangeRecordDto> GetChangeById([FromRoute] long id)
        // GET: grąžina pakeitimą pagal identifikatorių. Parametras gaunamas per route. (uzklausos DTO)
        {
            var change = _changeTracker.GetChangeById(id);
            if (change == null)
            {
                return NotFound($"Change with ID {id} not found.");
            }

            var result = _mapper.Map(change);
            return Ok(result);
        }

        [HttpGet("type/{changeType}")]
        public ActionResult<IEnumerable<ChangeRecordDto>> GetChangesByType([FromRoute] ChangeType changeType)
        // GET: grąžina pakeitimus pagal jų tipą. Parametras gaunamas per route. (uzklausos DTO)
        {
            var changes = _changeTracker.GetChangesByType(changeType);
            if (changes == null || !changes.Any())
            {
                return NotFound($"No changes found for type {changeType}.");
            }

            var result = _mapper.Map(changes);
            return Ok(result);
        }

        [HttpGet("range")]
        public ActionResult<IEnumerable<ChangeRecordDto>> GetChangesByDateRange([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        // GET: grąžina pakeitimus pagal laiko rėžius. Parametrai gaunami per query. (uzklausos DTO)
        {
            var changes = _changeTracker.GetChangesByDates(startTime, endTime);
            if (changes == null || !changes.Any())
            {
                return NotFound($"No changes found between {startTime} and {endTime}.");
            }

            var result = _mapper.Map(changes);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult TrackChange([FromBody] ChangeRecordCommandDto command)
        // POST: įrašo naują pakeitimą. Parametrai gaunami per body (komandos DTO).
        {
            if (command == null)
            {
                return BadRequest("Invalid change data.");
            }

            var changeRecord = _mapper.Map(command);
            _changeTracker.TrackChange(changeRecord);
            return CreatedAtAction(nameof(GetChangeById), new { id = changeRecord.Id }, _mapper.Map(changeRecord));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateChange([FromRoute] long id, [FromBody] ChangeRecordCommandDto command)
        // PUT: atnaujina pakeitimą pagal identifikatorių. Parametrai gaunami per body, o id per route (naudoti komandos DTO).
        {
            var change = _changeTracker.GetChangeById(id);
            if (change == null)
            {
                return NotFound($"Change with ID {id} not found.");
            }

            if (command == null)
            {
                return BadRequest("Invalid update data.");
            }

            if (string.IsNullOrEmpty(command.OldValue) || string.IsNullOrEmpty(command.NewValue) || string.IsNullOrEmpty(command.EntityName))
            {
                return BadRequest("Invalid update data.");
            }

            _mapper.Project(change, command);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult ClearAllChanges()
        // DELETE: ištrina visus įrašytus pakeitimus.
        {
            _changeTracker.ClearAllChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult ClearChangeById([FromRoute] long id)
        // DELETE: ištrina konkrečius pakeitimus.
        {
            var success = _changeTracker.ClearChangeById(id);
            if (!success)
            {
                return NotFound($"Change with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
