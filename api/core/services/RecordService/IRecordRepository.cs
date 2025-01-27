using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;

namespace api.core.services.RecordService
{
    public interface IRecordRepository
    {
        List<Record> GetAllRecords();
        Record GetRecordByID(int id);
        void CreateRecord(Record record);
        bool UpdateRecord(Record record, int id);
        void DeleteRecord(int id);
        bool SetRecordDeleted(int id);
    }
}