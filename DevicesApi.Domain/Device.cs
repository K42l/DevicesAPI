using System.Text.Json.Serialization;

namespace DevicesApi.Domain;

//I'm cutting some corners here because I'm short on time. So I'll explain my choices.
//I used a enum for the State to facilitate. Ideally this should be a separate class with an Id and Description
//I'm also using this class on Entity Framework directly. But it would be better to separate the domain model from the infrastructure model
public class Device
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Brand { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public DeviceStateEnum? State { get; set; }

    public void UpdateDevice(string? newName = null, string? newBrand = null, DeviceStateEnum? newState = null)
    {
        if (this.State == DeviceStateEnum.InUse)
        {
            if (this.Name != newName || this.Brand != newBrand)
                throw new InvalidOperationException("Cannot update name/brand when device is in use.");
        }

        this.Name = newName ?? this.Name;
        this.Brand = newBrand ?? this.Brand;
        if (newState != null)
            this.State = newState;
    }

    public void Delete()
    {
        if (this.State == DeviceStateEnum.InUse)
            throw new InvalidOperationException("Cannot delete a device that is in use.");
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeviceStateEnum
{
    Available = 1,
    InUse = 2,
    Inactive = 3
}