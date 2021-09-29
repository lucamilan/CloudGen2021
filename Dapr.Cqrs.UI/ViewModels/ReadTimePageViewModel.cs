using System.Collections.Generic;
using Dapr.Cqrs.Common.Models.Read;

namespace Dapr.Cqrs.UI.ViewModels
{
    public class ReadTimePageViewModel
    {
        public string ErrorMessage { get; set; }

        public RawDataDto RawData { get; set; }

        public List<SensorDto> SensorsData { get; set; } = new();
    }
}