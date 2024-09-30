using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightTracker.Models
{
    class UserProfile
    {
        public string Name { get; set; } = string.Empty;
        public double Height { get; set; } = 0;
        public double Weight { get; set; } = 0;
        public double RefWeight { get; set; } = 0;
        public DateTime RefDate { get; set; } = DateTime.MinValue;

        public UserProfile()
        {            
        }

        public UserProfile(string name, double height)
        {
            Name = name;
            Height = height;
        }
    }
}
