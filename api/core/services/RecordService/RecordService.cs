using api.core.models;

namespace api.core.services.RecordService
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _recordRepository;

        public RecordService(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task CreateRecordAsync(Record record)
        {
            await _recordRepository.CreateRecordAsync(record);
        }

        public async Task DeleteRecordAsync(int id)
        {
            await _recordRepository.DeleteRecordAsync(id);
        }

        public async Task<List<Record>> GetAllRecordsAsync()
        {
            return await _recordRepository.GetAllRecordsAsync();
        }

        public async Task<Record?> GetRecordByIdAsync(int id)
        {
            return await _recordRepository.GetRecordByIdAsync(id);
        }

        public async Task<List<Record>> GetRecordsByUserIdAsync(int id)
        {
            var records = await _recordRepository.GetRecordsByUserIdAsync(id);
            return records
                .OrderBy(r => (r.TimeMin * 60 * 1000) + (r.TimeSec * 1000) + r.TimeMs)
                .ToList();
        }

        public async Task<bool> SetRecordDeletedAsync(int id)
        {
            return await _recordRepository.SetRecordDeletedAsync(id);
        }

        public async Task<bool> UpdateRecordAsync(Record record, int id)
        {
            return await _recordRepository.UpdateRecordAsync(record, id);
        }
    }
}