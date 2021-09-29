namespace Dapr.Cqrs.Common.Models.Write
{
    public static class SensorDataExtensions
    {
        public static string ReadableValue(this SensorData sensorData) => sensorData.Tag == "TMP" ? $"{sensorData.Value} C" : $"{sensorData.Value}%";
        public static string ReadableData(this SensorData sensorData) => $"{SensorDataLookup.Plants[sensorData.Plant]} {SensorDataLookup.Locations[sensorData.Location]} {SensorDataLookup.Tags[sensorData.Tag]} {sensorData.ReadableValue()}";
    }
}