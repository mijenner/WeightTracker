namespace WeightTracker.Models
{
    class Measurement
    {
        public DateTime TimePoint { get; set; } = DateTime.MinValue;
        public double Weight { get; set; } = 0.0;

        public Measurement()
        {
        }

        public Measurement(double weight, DateTime timePoint)
        {
            Weight = weight;
            TimePoint = timePoint;
        }
    }
}
