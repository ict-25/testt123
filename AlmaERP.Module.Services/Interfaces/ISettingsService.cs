using AlmaERP.Module.BusinessObjects.Entities;

namespace AlmaERP.Module.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<SystemSettings> GetSettingsAsync();
        Task<SystemSettings> UpdateSettingsAsync(SystemSettings settings);
        Task<string> GetSettingValueAsync(string key);
        Task<bool> SetSettingValueAsync(string key, string value);
    }
}
