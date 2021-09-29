using System;
using System.Collections.Generic;
using System.Linq;
using Dapr.Cqrs.Common.Models.Read;
using Dapr.Cqrs.Common.Models.Write;
using Dapr.Cqrs.UI.ViewModels;

namespace Dapr.Cqrs.UI.Mappers
{
    public class DashboardViewModelMapper
    {
        public static List<PlantViewModel> Map(IEnumerable<AggregateDto> data)
        {
            var plants = new List<PlantViewModel>();

            var groupByPlant = data.GroupBy(_ => _.PlantId);

            foreach (var byPlant in groupByPlant)
            {
                var plant = new PlantViewModel() { Id = byPlant.Key, Name = SensorDataLookup.Plants[byPlant.Key] };
                var groupByPlantLocation = byPlant.GroupBy(_ => _.LocationId);

                foreach (var byPlantLocation in groupByPlantLocation)
                {
                    var location = new LocationViewModel() { Id = byPlantLocation.Key, Name = SensorDataLookup.Locations[byPlantLocation.Key] };
                    plant.Locations.Add(location);

                    var groupByPlantLocationTag = byPlantLocation.GroupBy(_ => _.TagId);

                    foreach (var byPlantLocationTag in groupByPlantLocationTag)
                    {
                        var tag = new TagViewModel() { Id = byPlantLocationTag.Key, Name = SensorDataLookup.Tags[byPlantLocationTag.Key] };
                        location.Tags.Add(tag);

                        foreach (var item in byPlantLocationTag)
                        {
                            tag.Intervals.AddRange(item.Values.Select(_ => new IntervalViewModel()
                            {
                                Id = _.IntervalId,
                                Average = _.Average,
                                Max = _.Max,
                                Min = _.Min,
                                Count = _.Count,
                                StandardDeviation = Math.Round(_.StandardDeviation,4)
                            }));
                        }
                    }
                }

                plants.Add(plant);
            }

            return plants;
        }
    }
}