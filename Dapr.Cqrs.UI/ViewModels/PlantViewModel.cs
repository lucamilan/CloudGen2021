using System.Collections.Generic;

namespace Dapr.Cqrs.UI.ViewModels
{
    public class PlantViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<LocationViewModel> Locations { get; set; } = new();
    }
}