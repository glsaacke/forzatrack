using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;

namespace api.core.services.RecordService
{
    public class RecordService : IRecordService
    {
        private IRecordRepository recordRepository;
        public RecordService(IRecordRepository recordRepository){
            this.recordRepository = recordRepository;
        }

        public void CreateRecord(Record record)
        {
            recordRepository.CreateRecord(record);
        }

        public void DeleteRecord(int id)
        {
            recordRepository.DeleteRecord(id);
        }

        public List<Record> GetAllRecords()
        {
            var records = recordRepository.GetAllRecords();
            return records;
        }

        public Record GetRecordByID(int id)
        {
            Record record = recordRepository.GetRecordByID(id);
            return record;
        }

        public bool SetRecordDeleted(int id)
        {
            bool rowsAffected = recordRepository.SetRecordDeleted(id);
            return rowsAffected;
        }

        public bool UpdateRecord(Record record, int id)
        {
            bool rowsAffected = recordRepository.UpdateRecord(record, id);
            return rowsAffected;
        }
    }
}