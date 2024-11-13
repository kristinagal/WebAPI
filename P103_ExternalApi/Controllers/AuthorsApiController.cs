using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P103_ExternalApi.Dtos;
using P103_ExternalApi.Services;

namespace P103_ExternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsApiController : ControllerBase
    {
        private readonly IAuthorsApiClient _authorsApiClient;
        private readonly IAuthorsMapper _mapper;

        public AuthorsApiController(IAuthorsApiClient authorsApiClient, IAuthorsMapper mapper)
        {
            _authorsApiClient = authorsApiClient;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AuthorResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthors([FromHeader] string connectionId)
        {
            var result = await _authorsApiClient.GetAuthors(connectionId);
            var authors = _mapper.Map(result!);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AuthorResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthor([FromHeader] string connectionId, [FromRoute] int id)
        {
            var result = await _authorsApiClient.GetAuthor(connectionId, id);
            if (result == null) return NotFound();
            var author = _mapper.Map(result);
            return Ok(author);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuthor([FromHeader] string connectionId, [FromBody] AuthorRequest req)
        {
            var apiRequest = _mapper.Map(req);
            var id = await _authorsApiClient.CreateAuthor(connectionId, apiRequest);
            return Created(nameof(GetAuthor), new { id });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAuthor([FromHeader] string connectionId, [FromRoute] int id, [FromBody] AuthorRequest req)
        {
            var author = await _authorsApiClient.GetAuthor(connectionId, id);
            if (author == null)
            {
                return NotFound();
            }

            var apiRequest = _mapper.Map(req);
            await _authorsApiClient.UpdateAuthor(connectionId, id, apiRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAuthor([FromHeader] string connectionId, [FromRoute] int id)
        {
            var author = await _authorsApiClient.GetAuthor(connectionId, id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorsApiClient.DeleteAuthor(connectionId, id);
            return NoContent();
        }
    }
}
