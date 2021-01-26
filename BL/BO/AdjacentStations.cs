using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class AdjacentStations
    {
        public int Station1 { get; set; }
        public int Station2 { get; set; }
        public double Distance { get; set; }
        public int TimeInMinutes { get; set; }
        public int TimeInHours { get; set; } 
        public TimeSpan Time 
        { 
            get => new TimeSpan(TimeInHours, TimeInMinutes, 0); 
        }
        public bool IsDeleted { get; set; }
        public string DistanceFormat
        {
            get
            {
                if (Distance > 1000)
                    return  String.Format("{0:0.0}", Distance / 1000) + " ק\"מ ";
                return String.Format("{0:0.0}", Distance) + " מ' ";
            }
        }
        public string TimeFormat
        {
            get
            {
                return Time.ToString(@"hh\:mm");
            }
        }
    }
}
