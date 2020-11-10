using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7588_3756
{
    public enum Regions { General, North, South, Center, Jerusalem, Ayosh };
    
    class BusLine : IComparable<BusLine>
    {
        public int BusLineNum { get; set; }

        public BusLineStation FirstStation { get; set; }

        public BusLineStation LastStation { get; set; }

        public List<BusLineStation> Stations { get; set; }

        public int Area { get; set; }

        public BusLine(int busLine, BusLineStation first, BusLineStation last)
        {
            BusLineNum = busLine;
            FirstStation = first;
            LastStation = last;
            Stations = new List<BusLineStation>
            {
                FirstStation,
                LastStation
            };
            Area = (int)Regions.General;
        }

        public BusLine()
        {
            Area = (int)Regions.General;
        }

        public override string ToString()
        {
            string output = $"Bus line number: {BusLineNum}\n";
            output += $"Area: {Area}\n";
            output += "Bus stations:\n";
            int i = 1;
            foreach(var station in Stations)
            {
                output += $"station number {i++} is {station.BusStationKey}\n";
            }
            return output;
        }

        public void AddStation(BusLineStation newSatation)
        {
            Stations.Insert(1, newSatation);
        }

        public void RemoveStation(BusLineStation satationToDel)
        {
            Stations.Remove(satationToDel);
        }

        public bool StationInTheRoute(BusLineStation satationToFind)
        {
            foreach(var station in Stations)
            {
                if (station.Equals(satationToFind))
                    return true;
            }
            return false;
        }

        public double DistanceBetweenStations(BusLineStation satation1, BusLineStation satation2)
        {
            if (!(StationInTheRoute(satation1) || StationInTheRoute(satation2)))
                return -1;
            
            return satation1.DistanceBetweenStations(satation2);
        }

        public TimeSpan? TimeBetweenStations(BusLineStation satation1, BusLineStation satation2)
        {
            if (!(StationInTheRoute(satation1) || StationInTheRoute(satation2)))
                return null;

            return satation1.TimeBetweenStations(satation2);
        }

        public BusLine SubRouteBeteenStation(BusLineStation first, BusLineStation second)
        {
            if (!(StationInTheRoute(first) || StationInTheRoute(second)))
                return null;

            BusLine subRoute = new BusLine();
            int index1 = getIndexOfStation(first);
            int index2 = getIndexOfStation(second);
            int smallIndex = Math.Min(index1, index2);
            int bigIndex = Math.Max(index1, index2);
            subRoute.Stations = Stations.GetRange(smallIndex, bigIndex - smallIndex);
            subRoute.FirstStation = first;
            subRoute.LastStation = second;

            return subRoute;
        }

        public int FastRoute(BusLine other)
        {
            int compare = this.CompareTo(other);
            
            if(compare != Int16.MaxValue)
            {
                if (compare == 1)
                    return other.BusLineNum;
                else
                    return this.BusLineNum;
            }
            
            return -1;
        }
        /// <summary>
        /// compare two BusLine by the duration time of the total drive 
        /// </summary>
        /// <param name="other"></param>
        /// <returns>One of the following value:
        /// -1 : if the total time of this is shorter then other,
        /// 0 : if thay equals,
        /// 1 : if the total time of this is longer then other,
        /// Int16.MaxValue : if it's erorr</returns>
        public int CompareTo(BusLine other)
        {
            TimeSpan? myLineTime = TimeBetweenStations(FirstStation, LastStation);
            TimeSpan? OtherLineTime = TimeBetweenStations(other.FirstStation, other.LastStation);
            
            if(myLineTime.HasValue && OtherLineTime.HasValue)
                return TimeSpan.Compare(myLineTime.Value, OtherLineTime.Value);
            
            return Int16.MaxValue;
        }


        private int getIndexOfStation(BusLineStation station)
        {
            int index = 0;
            foreach (var s in Stations)
            {
                if (s.Equals(station))
                    return index;
                index++;
            }
            return -1;
        }
    }
}
