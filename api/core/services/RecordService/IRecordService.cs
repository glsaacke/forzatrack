using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;

namespace api.core.services.RecordService
{
    public interface IRecordService
    {
        List<Record> GetAllRecords();
        Record GetRecordByID(int id);
        void CreateRecord(Record record);
        void UpdateRecord(Record record, int id);
        void DeleteRecord(int id);
    }
}