using Microsoft.AspNetCore.Mvc;
using P099_File.Models;
using P099_File.Repositories;

namespace P099_File.Services
{
    public interface IDbChangeTracker
    {
        void ClearAllChanges();
        bool ClearChangeById(long id);
        ChangeRecord? GetChangeById(long id);
        IEnumerable<ChangeRecord> GetChanges();
        IEnumerable<ChangeRecord> GetChangesByDates(DateTime startTime, DateTime endTime);
        IEnumerable<ChangeRecord> GetChangesByType(ChangeType changeType);
        void TrackChange(ChangeRecord change);
    }

    public class DbChangeTracker : IDbChangeTracker
    {
        private readonly IChangeRecordRepository _repository;
        private readonly ILogger<DbChangeTracker> _logger;

        public DbChangeTracker(IChangeRecordRepository repository, ILogger<DbChangeTracker> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void TrackChange(ChangeRecord change)
        {
            _repository.Add(change);
            _repository.SaveChanges();
            _logger.LogInformation($"Tracked change with ID: {change.Id}");
        }

        public ChangeRecord? GetChangeById(long id)
        {
            var change = _repository.GetById(id);
            _logger.LogInformation(change != null
                ? $"Retrieved change with ID: {id}"
                : $"Change with ID: {id} not found.");
            return change;
        }

        public IEnumerable<ChangeRecord> GetChanges()
        {
            _logger.LogInformation("Retrieving all changes.");
            return _repository.GetAll();
        }

        public IEnumerable<ChangeRecord> GetChangesByType(ChangeType changeType)
        {
            _logger.LogInformation($"Retrieving changes of type: {changeType}");
            return _repository.GetByType(changeType);
        }

        public IEnumerable<ChangeRecord> GetChangesByDates(DateTime startTime, DateTime endTime)
        {
            _logger.LogInformation($"Retrieving changes between {startTime} and {endTime}");
            return _repository.GetByDateRange(startTime, endTime);
        }

        public void ClearAllChanges()
        {
            _repository.RemoveRange(_repository.GetAll());
            _repository.SaveChanges();
            _logger.LogInformation("Cleared all change records.");
        }

        public bool ClearChangeById(long id)
        {
            var change = _repository.GetById(id);
            if (change != null)
            {
                _repository.Remove(change);
                _repository.SaveChanges();
                _logger.LogInformation($"Cleared change record with ID: {id}");
                return true;
            }

            _logger.LogWarning($"Attempted to clear change record with ID: {id}, but it was not found.");
            return false;
        }
    }
}
