using api.core.models;

namespace api.core.services.RecordService
{
    public interface IRecordRepository
    {
        Task<List<Record>> GetAllRecordsAsync();
        Task<Record?> GetRecordByIdAsync(int id);
        Task CreateRecordAsync(Record record);
        Task<bool> UpdateRecordAsync(Record record, int id);
        Task DeleteRecordAsync(int id);
        Task<bool> SetRecordDeletedAsync(int id);
        Task<List<Record>> GetRecordsByUserIdAsync(int id);
    }
}