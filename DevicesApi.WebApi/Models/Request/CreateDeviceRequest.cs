using DevicesApi.Domain;

namespace DevicesApi.WebApi.Models.Request
{
    public class CreateDeviceRequest
    {
        /// <summary>
        /// Device's name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Device's brand.
        /// </summary>
        public required string Brand { get; set; }

        /// <summary>
        /// Device's state. Available = 1, InUse = 2, Inactive = 3
        /// </summary>
        public required DeviceStateEnum DeviceState { get; set; }
    }
}
