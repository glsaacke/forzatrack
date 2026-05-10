using api.core.models;
using api.core.data;

namespace api.core.services.RecordService
{
    public class RecordRepository : IRecordRepository
    {
        private readonly AppDbContext _context;
        public RecordRepository(AppDbContext context){
            _context = context;
        }

        public List<Record> GetAllRecords(){
            return _context.Records.ToList();
        }

        public Record GetRecordByID(int id){
            return _context.Records.FirstOrDefault(r => r.RecordId == id);
        }

        public void CreateRecord(Record record){
            record.AddDate = DateTime.UtcNow;
            _context.Records.Add(record);
            _context.SaveChanges();
        }

        public bool UpdateRecord(Record record, int id){
            var existing = _context.Records.FirstOrDefault(r => r.RecordId == id);
            if (existing == null) return false;

            existing.UserId = record.UserId;
            existing.CarId = record.CarId;
            existing.Event = record.Event;
            existing.ClassRank = record.ClassRank;
            existing.TimeMin = record.TimeMin;
            existing.TimeSec = record.TimeSec;
            existing.TimeMs = record.TimeMs;
            existing.CpuDiff = record.CpuDiff;
            return _context.SaveChanges() > 0;
        }

        public void DeleteRecord(int id){
            var record = new Record { RecordId = id };
            _context.Records.Attach(record);
            _context.Records.Remove(record);
            _context.SaveChanges();
        }

        public bool SetRecordDeleted(int id){
            var existing = _context.Records.FirstOrDefault(r => r.RecordId == id);
            if (existing == null) return false;

            existing.Deleted = 1;
            return _context.SaveChanges() > 0;
        }

        public List<Record> GetRecordsByUserId(int id){
            return _context.Records.Where(r => r.UserId == id).ToList();
        }
    }
}
