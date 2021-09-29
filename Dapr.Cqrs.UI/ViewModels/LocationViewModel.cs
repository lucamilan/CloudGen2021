using System.Collections.Generic;

namespace Dapr.Cqrs.UI.ViewModels
{
    public class LocationViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TagViewModel> Tags { get; set; } = new();
    }
}
