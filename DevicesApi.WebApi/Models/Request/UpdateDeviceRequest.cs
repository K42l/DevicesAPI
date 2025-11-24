using DevicesApi.Domain;

namespace DevicesApi.WebApi.Models.Request
{
    public class UpdateDeviceRequest
    {
        /// <summary>
        /// Device's Id.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Device's name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Device's brand.
        /// </summary>
        public string? Brand { get; set; }

        /// <summary>
        /// Device's state.
        /// </summary>
        public DeviceStateEnum? DeviceState { get; set; }
    }
}
