using System;
using System.Collections.Generic;
using System.Linq;
using P099_File.Models;
using P099_File.Services;

namespace P099_File.Repositories
{
    public interface IChangeRecordRepository
    {
        void Add(ChangeRecord change);
        ChangeRecord? GetById(long id);
        IEnumerable<ChangeRecord> GetAll();
        IEnumerable<ChangeRecord> GetByType(ChangeType changeType);
        IEnumerable<ChangeRecord> GetByDateRange(DateTime startTime, DateTime endTime);
        void Remove(ChangeRecord change);
        void RemoveRange(IEnumerable<ChangeRecord> changes);
        void SaveChanges();
    }

    public class ChangeRecordRepository : IChangeRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public ChangeRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ChangeRecord change)
        {
            _context.ChangeRecords.Add(change);
        }

        public ChangeRecord? GetById(long id)
        {
            return _context.ChangeRecords.Find(id);
        }

        public IEnumerable<ChangeRecord> GetAll()
        {
            return _context.ChangeRecords.ToList();
        }

        public IEnumerable<ChangeRecord> GetByType(ChangeType changeType)
        {
            return _context.ChangeRecords.Where(c => c.ChangeType == changeType).ToList();
        }

        public IEnumerable<ChangeRecord> GetByDateRange(DateTime startTime, DateTime endTime)
        {
            return _context.ChangeRecords.Where(c => c.ChangeTime >= startTime && c.ChangeTime <= endTime).ToList();
        }

        public void Remove(ChangeRecord change)
        {
            _context.ChangeRecords.Remove(change);
        }

        public void RemoveRange(IEnumerable<ChangeRecord> changes)
        {
            _context.ChangeRecords.RemoveRange(changes);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
