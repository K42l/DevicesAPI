using DevicesApi.Domain;

namespace DevicesApi.Application;
public interface IDeviceRepository
{
    Task<Device> CreateAsync(Device device);
    Task<Device?> GetByIdAsync(Guid id);
    Task<IEnumerable<Device>> GetAllAsync();
    Task<IEnumerable<Device>> GetByBrandAsync(string brand);
    Task<IEnumerable<Device>> GetByStateAsync(DeviceStateEnum state);
    Task<IEnumerable<Device>> GetByFiltersAsync(string? brand = null, DeviceStateEnum? state = null);
    Task<Device?> UpdateAsync(Device device);
    Task<bool> DeleteAsync(Guid id);
}
