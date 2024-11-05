namespace P099_File.Services
{
    public interface IFileService
    {
        IEnumerable<string> ReadAllLines();
        string ReadLine(int lineNumber);
        void RemoveLine(int lineNumber);
        void ReplaceLine(int lineNumber, string newContent);
        void WriteLine(string line);
    }

    public class FileService : IFileService
    {
        private readonly string _filePath = "C:\\Users\\Studentas\\Desktop\\Mokymai\\repos\\WebApi\\DtoFailas.txt";

        public FileService()
        {
            // Ensure the file exists; if not, create it
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Dispose(); // Creates the file and closes the stream
            }
        }

        public void WriteLine(string line)
        {
            File.AppendAllText(_filePath, line + Environment.NewLine);
        }

        public string ReadLine(int lineNumber)
        {
            var lines = File.ReadAllLines(_filePath);
            if (lineNumber >= 0 && lineNumber < lines.Length)
            {
                return lines[lineNumber];
            }
            throw new IndexOutOfRangeException("Line number out of range");
        }

        public IEnumerable<string> ReadAllLines()
        {
            return File.ReadAllLines(_filePath);
        }

        public void ReplaceLine(int lineNumber, string newContent)
        {
            var lines = new List<string>(File.ReadAllLines(_filePath));
            if (lineNumber >= 0 && lineNumber < lines.Count)
            {
                lines[lineNumber] = newContent;
                File.WriteAllLines(_filePath, lines);
            }
            else
            {
                throw new IndexOutOfRangeException("Line number out of range");
            }
        }

        public void RemoveLine(int lineNumber)
        {
            var lines = new List<string>(File.ReadAllLines(_filePath));
            if (lineNumber >= 0 && lineNumber < lines.Count)
            {
                lines.RemoveAt(lineNumber);
                File.WriteAllLines(_filePath, lines);
            }
            else
            {
                throw new IndexOutOfRangeException("Line number out of range");
            }
        }
    }
}
