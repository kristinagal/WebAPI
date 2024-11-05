using Microsoft.AspNetCore.Mvc;
using P099_File.Dtos;
using P099_File.Services;
using System.Collections.Generic;

namespace P099_File.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IFileLineMapper _mapper;

        public FileController(IFileService fileService, IFileLineMapper mapper)
        {
            _fileService = fileService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FileLineDto>> GetAllLines()
        {
            var lines = _fileService.ReadAllLines();
            var dtoList = _mapper.Map(lines);
            return Ok(dtoList);
        }

        [HttpGet("{lineNumber}")]
        public ActionResult<FileLineDto> GetLine([FromRoute] int lineNumber)
        {
            try
            {
                var lineContent = _fileService.ReadLine(lineNumber);
                var dto = _mapper.Map(lineContent, lineNumber);
                return Ok(dto);
            }
            catch (IndexOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult WriteLine([FromBody] FileCommandDto text)
        {
            _fileService.WriteLine(text.Content);
            return CreatedAtAction(nameof(GetAllLines), new { }, text.Content);
        }

        [HttpPut("{lineNumber}")]
        public IActionResult ReplaceLine([FromRoute] int lineNumber, [FromBody] FileCommandDto text)
        {
            try
            {
                _fileService.ReplaceLine(lineNumber, text.Content);
                return NoContent();
            }
            catch (IndexOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{lineNumber}")]
        public IActionResult DeleteLine([FromRoute] int lineNumber)
        {
            try
            {
                _fileService.RemoveLine(lineNumber);
                return NoContent();
            }
            catch (IndexOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
