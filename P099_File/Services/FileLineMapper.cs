using P099_File.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace P099_File.Services
{
    public interface IFileLineMapper
    {
        string Map(FileLineDto dto);
        IEnumerable<FileLineDto> Map(IEnumerable<string> contents);
        FileLineDto Map(string content, int lineNumber);
        void Project(FileLineDto dto, ref string content);
    }

    public class FileLineMapper : IFileLineMapper
    {

        public FileLineDto Map(string content, int lineNumber)
        {
            return new FileLineDto
            {
                LineNumber = lineNumber,
                Content = content
            };
        }

        public IEnumerable<FileLineDto> Map(IEnumerable<string> contents)
        {
            return contents.Select((content, index) => Map(content, index));
        }

        public string Map(FileLineDto dto)
        {
            return dto.Content;
        }

        public void Project(FileLineDto dto, ref string content)
        {
            content = dto.Content;
        }
    }
}
