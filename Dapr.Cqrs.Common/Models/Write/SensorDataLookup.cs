using System.Collections.Generic;

namespace Dapr.Cqrs.Common.Models.Write
{
    public static class SensorDataLookup
    {
        public static readonly IDictionary<string, string> Locations = new Dictionary<string, string>
        {
            { "SLM", "Sala Macchine" },
            { "MEN", "Sala Mensa" },
            { "UFF", "Uffici Direzionali" }
        };
        public static readonly IDictionary<string, string> Plants = new Dictionary<string, string>
        {
            { "VE", "Verona" },
            { "PC", "Piacenza" },
        };
        public static readonly IDictionary<string, string> Tags = new Dictionary<string, string>
        {
            { "TMP", "Temperatura" },
            { "HUM", "Umidità" }
        };
    }
}