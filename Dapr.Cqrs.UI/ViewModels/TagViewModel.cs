using System.Collections.Generic;

namespace Dapr.Cqrs.UI.ViewModels
{
    public class TagViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<IntervalViewModel> Intervals { get; set; } = new();

    }
}
