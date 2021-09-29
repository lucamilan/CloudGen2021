namespace Dapr.Cqrs.UI.ViewModels
{
    public class IntervalViewModel
    {
        public string Id { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Average { get; set; }
        public double StandardDeviation { get; set; }
        public int Count { get; set; }

        public static string AsLabel(int id)
        {
            switch (id)
            {
                case 0:
                    return "30 sec.";
                case 1:
                    return "60 sec.";
                case 2:
                    return "2 min.";
                case 3:
                    return "5 min.";

            }  

            return "30 sec";
        }
    }
}
