using System;
using System.Collections.Generic;
using System.Linq;


namespace dotNet5781_02_7588_3756
{
    public enum Regions { General, North, South, Center, Jerusalem };//enum of region in israel
    /// <summary>
    /// class BusLine
    /// </summary>
    public class BusLine : IComparable<BusLine>
    {
        /// <summary>
        /// the bus line number
        /// </summary>
        public int BusLineNum { get; set; }
        
        /// <summary>
        /// the first station in the line
        /// </summary>
        public BusLineStation FirstStation { get; set; }
        
        /// <summary>
        /// the last station in the line
        /// </summary>
        public BusLineStation LastStation { get; set; }
        
        /// <summary>
        /// all station in the line
        /// </summary>
        public List<BusLineStation> Stations { get; set; }
        
        /// <summary>
        /// the area in the contry of the line
        /// </summary>
        public Regions Area { get; set; }
        
        /// <summary>
        /// the totla time of the line from the first station to the last
        /// </summary>
        public TimeSpan? TotalTimeOfTheLine { get; set; }

        public List<TimeSpan?> TimeListOfTheStation { get; set; }

        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="busLine"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        public BusLine(int busLine, BusLineStation first, BusLineStation last)
        {
            BusLineNum = busLine;
            FirstStation = new BusLineStation(first);
            LastStation = new BusLineStation(last);
            Stations = new List<BusLineStation>
            {
                FirstStation,
                LastStation
            };
            Area = GetRegion(FirstStation.MyLocation.X, LastStation.MyLocation.Y);
        }
        /// <summary>
        /// copy constractor
        /// </summary>
        /// <param name="other"></param>
        public BusLine(BusLine other)
        {
            BusLineNum = other.BusLineNum;
            FirstStation = new BusLineStation(other.FirstStation);
            LastStation = new BusLineStation(other.LastStation);
            Stations = new List<BusLineStation>
            {
                FirstStation,
                LastStation
            };
            Area = GetRegion(FirstStation.MyLocation.X, LastStation.MyLocation.Y);
        }
        /// <summary>
        /// empty constractor
        /// </summary>
        public BusLine() { }
        /// <summary>
        /// override ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string output = $"Bus line number: {BusLineNum}\n";
            output += $"Area: {Area}\n";
            output += "Bus stations:\n";
            int i = 1;
            foreach (var station in Stations)
            {
                output += $"Station number {i++} is {station.BusStationKey}\n";
            }
            return output;
        }
        /// <summary>
        /// add station to the bus line
        /// </summary>
        /// <param name="newSatation"></param>
        public void AddStation(BusLineStation newSatation)
        {
            Stations.Insert(Stations.Count - 1, newSatation);
            LastStation.DistanceFromPrevStation = DistanceBetweenStations(LastStation, newSatation);
            newSatation.DistanceFromPrevStation = DistanceBetweenStations(newSatation,
                Stations.ElementAt(Stations.Count - 2));
            int hours = (int)LastStation.DistanceFromPrevStation % 24;
            int minutes = (int)LastStation.DistanceFromPrevStation % 60;
            LastStation.TimeFromPrevStation = new TimeSpan(hours, minutes, 0);//updete the time of the last station
            hours = (int)newSatation.DistanceFromPrevStation % 24;
            minutes = (int)newSatation.DistanceFromPrevStation % 60;
            newSatation.TimeFromPrevStation = new TimeSpan(hours, minutes, 0);
            updeteTotalTime();
        }

        private void updeteTotalTime()
        {
            foreach(var s in Stations)
            {
                TotalTimeOfTheLine?.Add((TimeSpan)s.TimeFromPrevStation);
            }
        }

        /// <summary>
        /// remove station from the bus line
        /// </summary>s
        /// <param name="satationToDel"></param>
        public void RemoveStation(BusLineStation satationToDel)
        {
            if (!StationInTheRoute(satationToDel))
                return;
            if(satationToDel != LastStation)
            {
                var next =  Stations.ElementAt(Stations.IndexOf(satationToDel) + 1);
                next.TimeFromPrevStation?.Add((TimeSpan)satationToDel.TimeFromPrevStation);
                next.DistanceFromPrevStation += satationToDel.DistanceFromPrevStation;
            }
            else
            {
                LastStation.TimeFromPrevStation?.Add((TimeSpan)satationToDel.TimeFromPrevStation);
                LastStation.DistanceFromPrevStation += satationToDel.DistanceFromPrevStation;
            }
            
            Stations.Remove(satationToDel);
            updeteTotalTime();
        }
        /// <summary>
        /// check if a given station is in the routh of the bus line
        /// </summary>
        /// <param name="satationToFind"></param>
        /// <returns></returns>
        public bool StationInTheRoute(BusLineStation satationToFind)
        {
            foreach (var station in Stations)
            {
                if (station.Equals(satationToFind))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// get the distance between two stations
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        /// <returns></returns>
        public double DistanceBetweenStations(BusLineStation station1, BusLineStation station2)
        {
            if (!StationInTheRoute(station1) && !StationInTheRoute(station2))
                return -1;

            return station1.DistanceBetweenStations(station2);
        }
        /// <summary>
        /// get the time between two stations
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        /// <returns>the total time of sub rout between the station</returns>
        public TimeSpan? TimeBetweenStations(BusLineStation station1, BusLineStation station2)
        {
            return SubRouteBetweenStation(station1, station2).TotalTimeOfTheLine;
        }
        /// <summary>
        /// get a sub rout between tow stations in the line
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public BusLine SubRouteBetweenStation(BusLineStation first, BusLineStation second)
        {
            if (!StationInTheRoute(first) && !StationInTheRoute(second))
                return null;

            BusLine subRoute = new BusLine();
            int index1 = getIndexOfStation(first);
            int index2 = getIndexOfStation(second);
            int smallIndex = Math.Min(index1, index2);
            int bigIndex = Math.Max(index1, index2);
            subRoute.Stations = Stations.GetRange(smallIndex, bigIndex - smallIndex);
            subRoute.FirstStation = new BusLineStation(first);
            subRoute.LastStation = new BusLineStation(second);
            subRoute.Area = GetRegion(subRoute.FirstStation.MyLocation.X,
                subRoute.LastStation.MyLocation.Y);

            return subRoute;
        }
        /// <summary>
        /// get the fast route between two lines
        /// </summary>
        /// <param name="other"></param>
        /// <returns>the number of line of the faster</returns>
        public int FastRoute(BusLine other)
        {
            int compare = this.CompareTo(other);

            if (compare == 1)
                return other.BusLineNum;
            return this.BusLineNum;
        }
        /// <summary>
        /// compare two BusLine by the duration time of the total drive 
        /// </summary>
        /// <param name="other"></param>
        /// <returns>One of the following value:
        /// -1 : if the total time of this is shorter then other,
        /// 0 : if thay equals,
        /// 1 : if the total time of this is longer then other</returns>
        public int CompareTo(BusLine other)
        {
            return TimeSpan.Compare(this.TotalTimeOfTheLine.Value, other.TotalTimeOfTheLine.Value);
        }
        /// <summary>
        /// compare by the bus line number
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
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
        public Regions GetRegion(double latitude, double longitude)
        {
            if (ValidInput.InRange(latitude, longitude, 34.461262, 35.2408, 31.83538, 32.26356))
                return Regions.Center;

            if (ValidInput.InRange(latitude, longitude, 35.1252, 35.2642, 31.7082, 31.8830))
                return Regions.Jerusalem;

            if (ValidInput.InRange(latitude, longitude, 34.8288611, 35.97865, 32.3508222, 33.3579972))
                return Regions.North;

            if (ValidInput.InRange(latitude, longitude, 33.81264994, 35.5857, 29.47925, 30.493059))
                return Regions.South;

            return Regions.General;
        }
        /// <summary>
        /// Get station by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BusLineStation GetStationByKey(int key)
        {
            return Stations.Find(x => x.BusStationKey == key);
        }

        public void PrintStationInfo()
        {
            Stations.ForEach(Console.WriteLine);
        }
    }
}
