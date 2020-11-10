using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7588_3756
{
    public enum Regions { General, North, South, Center, Jerusalem };

    class BusLine : IComparable<BusLine>
    {
        public int BusLineNum { get; set; }

        public BusLineStation FirstStation { get; set; }

        public BusLineStation LastStation { get; set; }

        public List<BusLineStation> Stations { get; set; }

        public int Area { get; set; }

        public TimeSpan? TotalTimeOfTheLine { get; set; }

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
            Area = GetRegion(FirstStation.X, LastStation.Y);
        }

        public BusLine()
        {
        }

        public override string ToString()
        {
            string output = $"Bus line number: {BusLineNum}\n";
            output += $"Area: {Area}\n";
            output += "Bus stations:\n";
            int i = 1;
            foreach(var station in Stations)
            {
                output += $"Station number {i++} is {station.BusStationKey}\n";
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

        public double DistanceBetweenStations(BusLineStation station1, BusLineStation station2)
        {
            if (!StationInTheRoute(station1) || !StationInTheRoute(station2))
                return -1;
            
            return station1.DistanceBetweenStations(station2);
        }

        public TimeSpan? TimeBetweenStations(BusLineStation station1, BusLineStation station2)
        {
            return SubRouteBeteenStation(station1, station2).TotalTimeOfTheLine;
        }

        public BusLine SubRouteBeteenStation(BusLineStation first, BusLineStation second)
        {
            if (!StationInTheRoute(first) || !StationInTheRoute(second))
                return null;

            BusLine subRoute = new BusLine();
            int index1 = getIndexOfStation(first);
            int index2 = getIndexOfStation(second);
            int smallIndex = Math.Min(index1, index2);
            int bigIndex = Math.Max(index1, index2);
            subRoute.Stations = Stations.GetRange(smallIndex, bigIndex - smallIndex);
            subRoute.FirstStation = first;
            subRoute.LastStation = second;
            subRoute.Area = GetRegion(subRoute.FirstStation.X, subRoute.LastStation.Y);

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
            if (this.TotalTimeOfTheLine.HasValue && other.TotalTimeOfTheLine.HasValue)
                return TimeSpan.Compare(this.TotalTimeOfTheLine.Value, other.TotalTimeOfTheLine.Value);

            return Int16.MaxValue;
        }

        public bool Equals(BusLine other)
        {
            return this.BusLineNum == other.BusLineNum;
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
        /// <summary>
        /// According to Wikipedia 
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public int GetRegion(double latitude, double longitude)
        {
            if (inRange(latitude, longitude, 34.461262, 35.2408, 31.83538, 32.26356))
                return (int)Regions.Center;
            if (inRange(latitude, longitude, 35.1252, 35.2642, 31.7082, 31.8830))
                return (int)Regions.Jerusalem;
            if (inRange(latitude, longitude, 34.8288611, 35.97865, 32.3508222, 33.3579972))
                return (int)Regions.North;
            if (inRange(latitude, longitude, 33.81264994, 35.5857, 29.47925, 30.493059))
                return (int)Regions.South;
            return (int)Regions.General;
        }

        private bool inRange(double param1, double param2,  double x1, double y1, double x2, double y2)
        {
            return param1 >= x1 && param1 <= y1 && param2 >= x2 && param2 <= y2;
        }
    }
}
