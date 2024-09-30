using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightTracker.Models
{
    class Measurement
    {
        public double Weight { get; set; } = 0.0;
        public DateTime Date { get; set; } = DateTime.MinValue; 

        public Measurement()
        {
        }

        public Measurement(double weight, DateTime date)
        {
            Weight = weight;
            Date = date;
        }
    }
}
