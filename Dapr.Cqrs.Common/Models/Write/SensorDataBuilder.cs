using System;
using System.Collections.Generic;

namespace Dapr.Cqrs.Common.Models.Write
{
    public static class SensorDataBuilder
    {
        public static IEnumerable<SensorData> BuildNonRandom()
        {
            var dateTime = DateTime.Now;
            foreach (var plantsKey in SensorDataLookup.Plants.Keys)
            {
                foreach (var locationsKey in SensorDataLookup.Locations.Keys)
                {
                    var milliseconds = RandomNumberBetween(0, 1000);
                    foreach (var tagsKey in SensorDataLookup.Tags.Keys)
                    {
                        var recordedOn = dateTime.AddSeconds(-1).AddMilliseconds(milliseconds);

                        yield return new SensorData
                        {
                            Location = locationsKey,
                            Plant = plantsKey,
                            Tag = tagsKey,
                            Value = GetTagValue(tagsKey),
                            RecordedOn = recordedOn
                        };
                    }
                }
            }
        }

        private static readonly Random RandomSeed = new();

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = RandomSeed.NextDouble();

            return minValue + next * (maxValue - minValue);
        }

        private static double GetTagValue(string tag)
        {
            //Temperatura
            var tempValue = RandomNumberBetween(15, 28);

            //Umidità relativa
            var humValue = RandomNumberBetween(25, 50);

            if (tempValue < 20)
                humValue = RandomNumberBetween(50, 90);

            if (tempValue > 25)
                humValue = RandomNumberBetween(20, 30);

            return tag switch
            {
                "TMP" => Math.Round(tempValue, 2),
                _ => Math.Round(humValue, 2)
            };
        }
    }
}