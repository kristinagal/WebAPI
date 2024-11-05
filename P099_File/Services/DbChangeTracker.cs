using P099_File.Models;

namespace P099_File.Services
{
    public interface IDbChangeTracker
    {
        void TrackChange(ChangeRecord change);
        ChangeRecord GetChangeById(long id);
        IEnumerable<ChangeRecord> GetChanges();
        IEnumerable<ChangeRecord> GetChangesByType(ChangeType changeType);
        IEnumerable<ChangeRecord> GetChangesByDates(DateTime startTime, DateTime endTime);
        void ClearAllChanges();
        bool ClearChangeById(long id);
    }

    public class DbChangeTracker : IDbChangeTracker
    {
        private readonly List<ChangeRecord> _changeRecords = new List<ChangeRecord>();

        public void TrackChange(ChangeRecord change)
        {
            throw new NotImplementedException();
        }

        public ChangeRecord GetChangeById(long id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ChangeRecord> GetChanges()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChangeRecord> GetChangesByType(ChangeType changeType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChangeRecord> GetChangesByDates(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public void ClearAllChanges()
        {
            throw new NotImplementedException();
        }

        public bool ClearChangeById(long id)
        {
            var change = _changeRecords.FirstOrDefault(c => c.Id == id);
            if (change != null)
            {
                _changeRecords.Remove(change);
                return true;
            }
            return false;
        }
    }
}
