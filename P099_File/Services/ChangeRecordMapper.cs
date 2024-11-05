using P099_File.Dtos;
using P099_File.Models;
using System.Collections.Generic;
using System.Linq;

namespace P099_File.Services
{
    public interface IChangeRecordMapper
    {
        ChangeRecordDto Map(ChangeRecord record);
        IEnumerable<ChangeRecordDto> Map(IEnumerable<ChangeRecord> records);
        ChangeRecord Map(ChangeRecordCommandDto commandDto);
        void Project(ChangeRecord record, ChangeRecordCommandDto commandDto);
    }

    public class ChangeRecordMapper : IChangeRecordMapper
    {
        public ChangeRecordDto Map(ChangeRecord record)
        {
            return new ChangeRecordDto
            {
                Id = record.Id,
                EntityName = record.EntityName,
                ChangeType = record.ChangeType,
                OldValue = record.OldValue,
                NewValue = record.NewValue,
                ChangeTime = record.ChangeTime
            };
        }

        public IEnumerable<ChangeRecordDto> Map(IEnumerable<ChangeRecord> records)
        {
            return records.Select(Map);
        }

        public ChangeRecord Map(ChangeRecordCommandDto commandDto)
        {
            return new ChangeRecord
            {
                EntityName = commandDto.EntityName,
                ChangeType = commandDto.ChangeType,
                OldValue = commandDto.OldValue,
                NewValue = commandDto.NewValue,
                ChangeTime = DateTime.Now
            };
        }

        public void Project(ChangeRecord record, ChangeRecordCommandDto commandDto)
        {
            record.EntityName = commandDto.EntityName;
            record.ChangeType = commandDto.ChangeType;
            record.OldValue = commandDto.OldValue;
            record.NewValue = commandDto.NewValue;
            record.ChangeTime = DateTime.Now;
        }
    }
}
