using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// 
    /// </summary>
    public class LineTrip
    {   
        public int Id { get; set; }
        public int LineId { get; set; }
        public int LineCode { get; set; }
        public int StartAtInMinutes { get; set; }
        public int StartAtInHours { get; set; }
        public string LastStation { get; set; }
        public TimeSpan StartAt 
        {
            get => new TimeSpan(StartAtInHours, StartAtInMinutes, 0);
        }
        public TimeSpan CurrentTimeToStation { get; set; }
        public string CurrentTimeToStationFormat 
        {
            get => CurrentTimeToStation.ToString(@"hh\:mm");
        }
        public TimeSpan ArrivalTimeToStation { get; set; }
        public string ArrivalTimeToStationFormat
        {
            get => ArrivalTimeToStation.ToString(@"hh\:mm");
        }
    }
}
