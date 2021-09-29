using System.Collections.Generic;

namespace Dapr.Cqrs.UI.ViewModels
{
    public class DashboardViewModel
    {
        public string ErrorMessage { get; set; }
        public string DataInsertedCounter { get; set; }
        public string DataRawProcessingCounter { get; set; }
        public string DataRawProcessingLastId { get; set; }
        public string DataTimeProcessingCounter { get; set; }
        public string DataSearchProcessingCounter { get; set; }
        public string DataRetriedOutboxCounter { get; set; }
        public string DataRemovedOutboxCounter { get; set; }

        public List<PlantViewModel> Plants { get; set; } = new();
    }
}