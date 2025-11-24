//Notes:
//It would be better to create a CustomErrorResponse for the error handling instead of returning the exception messages directly.
//Also I created some redundant endpoints for demonstration purposes.

using DevicesApi.Application;
using DevicesApi.Domain;
using DevicesApi.WebApi.Models.Request;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// RESTful API for Device CRUD Operations
/// operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceRepository _deviceRepository;
    public DevicesController(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <remarks>Creates a new device.</remarks>
    /// <param name="createDeviceRequest">Device creation request.</param>
    [HttpPost("Create")]
    [ProducesResponseType(typeof(Device), 201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<Device>> Create(CreateDeviceRequest createDeviceRequest)
    {
        try
        {
            var device = new Device
            {
                Id = Guid.NewGuid(),
                Name = createDeviceRequest.Name,
                Brand = createDeviceRequest.Brand,
                State = createDeviceRequest.DeviceState,
                CreatedAt = DateTime.UtcNow
            };
            var createdDevice = await _deviceRepository.CreateAsync(device);
            return Created("/devices/Create", createdDevice);
        }
        catch (Exception e)
        {
            //I'm returning the exception message for simplicity but, this exception should be handled, logged and returned properly.
            return StatusCode(500, new { e.Message });
        }
    }

    /// <summary>
    /// GetById
    /// </summary>
    /// <remarks>Retrieves a device by its Id.</remarks>
    /// <param name="id">Device's Id.</param>
    [HttpGet("GetById/{id}")]
    [ProducesResponseType(typeof(Device), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<Device>> GetById(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            return ValidationProblem("Invalid device ID.");

        try
        {
            var device = await _deviceRepository.GetByIdAsync(guid);
            if (device == null)
                return NotFound();

            return Ok(device);
        }
        catch (Exception e)
        {
            //I'm returning the exception message for simplicity. This exception should be handled, logged and returned properly.
            return StatusCode(500, new { e.Message });
        }
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <remarks>Retrieves all devices.</remarks>
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(Device), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<Device>>> GetAll()
    {
        try
        {
            var devices = await _deviceRepository.GetAllAsync();
            return Ok(devices);
        }
        catch (Exception e)
        {
            //I'm returning the exception message for simplicity. This exception should be handled, logged and returned properly.
            return StatusCode(500, new { e.Message });
        }
    }

    /// <summary>
    /// GetByBrand
    /// </summary>
    /// <remarks>Retrieves the devices that match the specified brand.</remarks>
    /// <param name="brand">Device's brand.</param>
    [HttpGet("GetByBrand/{brand}")]
    [ProducesResponseType(typeof(Device), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<Device>>> GetByBrand(string brand)
    {
        if (string.IsNullOrEmpty(brand))
            return ValidationProblem("Invalid brand.");

        try
        {
            var devices = await _deviceRepository.GetByBrandAsync(brand);
            return Ok(devices);
        }
        catch (Exception e)
        {
            //I'm returning the exception message for simplicity. This exception should be handled, logged and returned properly.
            return StatusCode(500, new { e.Message });
        }
    }

    /// <summary>
    /// GetByState
    /// </summary>
    /// <remarks>Retrieves the devices that match the specified state.</remarks>
    /// <param name="state">Device's State. Available = 1, InUse = 2, Inactive = 3.</param>
    [HttpGet("GetByState/{state}")]
    [ProducesResponseType(typeof(Device), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<Device>>> GetByState(DeviceStateEnum state)
    {
        if(string.IsNullOrEmpty(state.ToString()))
            return ValidationProblem("Invalid state.");

        try
        {
            var devices = await _deviceRepository.GetByStateAsync(state);
            return Ok(devices);
        }
        catch (Exception e)
        {
            //I'm returning the exception message for simplicity. This exception should be handled, logged and returned properly.
            return StatusCode(500, new { e.Message });
        }
    }

    /// <summary>
    /// GetByFilters
    /// </summary>
    /// <remarks>Retrieves the devices that match the specified brand and state filters.</remarks>
    /// <param name="brand">Device's brand. If null, devices of all brands are included.</param>
    /// <param name="state">Device's state. Available = 1, InUse = 2, Inactive = 3. If null, devices in all states are included.</param>
    [HttpGet("GetByFilters")]
    [ProducesResponseType(typeof(Device), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<Device>>> GetByFilters(string? brand = null, DeviceStateEnum? state = null)
    {
        try
        {
            var devices = await _deviceRepository.GetByFiltersAsync(brand, state);
            return Ok(devices);
        }
        catch (Exception e)
        {
            //I'm returning the exception message for simplicity. This exception should be handled, logged and returned properly.
            return StatusCode(500, new { e.Message });
        }
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <remarks>Updates the device by the provided Id.</remarks>
    /// <param name="id">Device's Id. Must match the Id provided in the request body.</param>
    /// <param name="updateDeviceRequest">Device update request.</param>
    [HttpPut("Update/{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update(string id, UpdateDeviceRequest updateDeviceRequest)
    {
        if (id != updateDeviceRequest.Id)
            return ValidationProblem("ID in the path does not match ID in the request body.");

        if(!Guid.TryParse(updateDeviceRequest.Id, out var guid))
            return ValidationProblem("Invalid device ID.");

        try
        {
            var device = await _deviceRepository.GetByIdAsync(guid);
            if (device == null)
                return NotFound();

            var updateDeviceDomain = new Device()
            {
                Id = guid,
                Name = updateDeviceRequest.Name ?? device.Name,
                Brand = updateDeviceRequest.Brand ?? device.Brand,
                State = updateDeviceRequest.DeviceState ?? device.State,
                CreatedAt = device.CreatedAt
            };

            var result = await _deviceRepository.UpdateAsync(updateDeviceDomain);
            if (result == null)
                return NotFound();
        }
        catch (InvalidOperationException e)
        {
            return ValidationProblem(e.Message);
        }
        catch (Exception e)
        {
            //I'm returning the exception message for simplicity. This exception should be handled, logged and returned properly.
            return StatusCode(500, new { e.Message });
        }
        return NoContent();
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <remarks>Deletes the device with the specified Id.</remarks>
    /// <param name="id">Device's Id to delete. Cannot be null or empty.</param>
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(string id)
    {
        if(!Guid.TryParse(id, out var guid))
            return ValidationProblem("Invalid device ID.");

        try
        {
            var device = await _deviceRepository.GetByIdAsync(guid);
            if (device == null)
                return NotFound();

            var result = await _deviceRepository.DeleteAsync(guid);
            if (!result)
                return NotFound();
        }
        catch (InvalidOperationException e)
        {
            return ValidationProblem(e.Message);
        }
        catch (Exception e)
        {
            //I'm returning the exception message for simplicity. This exception should be handled, logged and returned properly.
            return StatusCode(500, new { e.Message });
        }
        return NoContent();
    }
}