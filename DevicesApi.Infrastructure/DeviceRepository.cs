using DevicesApi.Application;
using DevicesApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevicesApi.Infrastructure;
public class DeviceRepository : IDeviceRepository
{
    private readonly DeviceDbContext _dbContext;
    public DeviceRepository(DeviceDbContext context)
    {
        _dbContext = context;
    }

    public async Task<Device> CreateAsync(Device device)
    {
        device.CreatedAt = DateTime.UtcNow;

        _dbContext.Devices.Add(device);
        await _dbContext.SaveChangesAsync();

        return device;
    }

    public async Task<Device?> GetByIdAsync(Guid id) =>
        await _dbContext.Devices.FindAsync(id);
    public async Task<IEnumerable<Device>> GetAllAsync() =>
        await _dbContext.Devices.ToArrayAsync();
    public async Task<IEnumerable<Device>> GetByBrandAsync(string brand) =>
        await GetByFiltersAsync(brand: brand);
    public async Task<IEnumerable<Device>> GetByStateAsync(DeviceStateEnum state) =>
        await GetByFiltersAsync(state: state);


    public async Task<IEnumerable<Device>> GetByFiltersAsync(string? brand = null, DeviceStateEnum? state = null)
    {
        var query = _dbContext.Devices.AsQueryable();

        if (!string.IsNullOrEmpty(brand))
            query = query.Where(x => x.Brand == brand);

        if (state.HasValue)
            query = query.Where(x => x.State == state.Value);

        return await query.ToArrayAsync();
    }

    public async Task<Device?> UpdateAsync(Device updateDevice)
    {
        var device = await _dbContext.Devices.FindAsync(updateDevice.Id);
        if (device == null) 
            return null;

        device.UpdateDevice(updateDevice.Name, updateDevice.Brand, updateDevice.State);

        await _dbContext.SaveChangesAsync();
        return device;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var device = await _dbContext.Devices.FindAsync(id);
        if (device == null) 
            return false;

        device.Delete();

        _dbContext.Devices.Remove(device);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
