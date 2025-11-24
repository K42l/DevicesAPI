using DevicesApi.Domain;

namespace DevicesApi.Tests;

[TestFixture]
public class DeviceTests
{
    private static Device CreateDevice( string name = "test name", string brand = "test brand", DeviceStateEnum state = DeviceStateEnum.Available)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Brand = brand,
            State = state,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    [Test]
    public void UpdateDevice_UpdateNameBrandState()
    {
        var device = CreateDevice();

        device.UpdateDevice("test update", "test update", DeviceStateEnum.InUse);

        Assert.That(device.Name, Is.EqualTo("test update"));
        Assert.That(device.Brand, Is.EqualTo("test update"));
        Assert.That(device.State, Is.EqualTo(DeviceStateEnum.InUse));
    }

    [Test]
    public void UpdateDevice_UpdateDeviceName()
    {
        var device = CreateDevice();

        device.UpdateDevice(newName: "test update");

        Assert.That(device.Name, Is.EqualTo("test update"));
        Assert.That(device.Brand, Is.EqualTo("test brand"));
        Assert.That(device.State, Is.EqualTo(DeviceStateEnum.Available));
    }

    [Test]
    public void UpdateDevice_UpdateDeviceBrand()
    {
        var device = CreateDevice();

        device.UpdateDevice(newBrand: "test update");

        Assert.That(device.Name, Is.EqualTo("test name"));
        Assert.That(device.Brand, Is.EqualTo("test update"));
        Assert.That(device.State, Is.EqualTo(DeviceStateEnum.Available));
    }

    [Test]
    public void UpdateDevice_UpdateDeviceState()
    {
        var device = CreateDevice();

        device.UpdateDevice(newState: DeviceStateEnum.Inactive);

        Assert.That(device.Name, Is.EqualTo("test name"));
        Assert.That(device.Brand, Is.EqualTo("test brand"));
        Assert.That(device.State, Is.EqualTo(DeviceStateEnum.Inactive));
    }

    [Test]
    public void UpdateDevice_UpdateWithNullParametersDoesntChange()
    {
        var device = CreateDevice();

        device.UpdateDevice();

        Assert.That(device.Name, Is.EqualTo("test name"));
        Assert.That(device.Brand, Is.EqualTo("test brand"));
        Assert.That(device.State, Is.EqualTo(DeviceStateEnum.Available));
    }

    [Test]
    public void UpdateDevice_UpdateDeviceNameInUse_Throw()
    {
        var device = CreateDevice(state: DeviceStateEnum.InUse);

        Assert.Throws<InvalidOperationException>(() => device.UpdateDevice(newName: "test throw"));
    }

    [Test]
    public void UpdateDevice_UpdateDeviceBrandInUse_Throw()
    {
        var device = CreateDevice(state: DeviceStateEnum.InUse);

        Assert.Throws<InvalidOperationException>(() => device.UpdateDevice(newBrand: "test throw"));
    }

    [Test]
    public void CheckIfCanDelete_AvailableDevice()
    {
        var device = CreateDevice(state: DeviceStateEnum.Available);

        Assert.DoesNotThrow(device.Delete);
    }

    [Test]
    public void CheckIfCanDelete_InactiveDevice()
    {
        var device = CreateDevice(state: DeviceStateEnum.Inactive);

        Assert.DoesNotThrow(device.Delete);
    }

    [Test]
    public void CheckIfCanDelete_DeviceInUse_Throw()
    {
        var device = CreateDevice(state: DeviceStateEnum.InUse);

        Assert.Throws<InvalidOperationException>(device.Delete);
    }
}