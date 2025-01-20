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
        Record GetCarByID(int id);
        void CreateRecord(Record record);
        void Update(Record record);
        void Delete(int id);
    }
}