using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineStation
    {
        public int LineId { get; set; }
        public int Station { get; set; }
        public int LineStationIndex { get; set; }
        public int PrevStation { get; set; }
        public int NextStation { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public double DistanceFromNext { get; set; }
        public TimeSpan TimeFromNext { get; set; }
        public double DistanceFromPrev { get; set; }
        public TimeSpan TimeFromPrev { get; set; }
        
        public string DistanceFormat
        {
            get
            {
                if (DistanceFromNext > 1000)
                    return String.Format("{0:0.0}", DistanceFromNext / 1000) + " ק\"מ ";
                return String.Format("{0:0.0}", DistanceFromNext) + "מ'";
            }
        }
        public string TimeFormat
        {
            get
            {
                return TimeFromNext.ToString(@"hh\:mm");
            }
        }

    }
}
