using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7588_3756
{


    class BusLineStation : BusStation
    {
        public double DistanceFromPrevStation { get; set; }
        public TimeSpan? TimeFromPrevStation { get; set; }
        public BusLineStation PrevStation { get; set; }

        public BusLineStation(string key, double latitude, double longitude)
            : base(key, latitude, longitude)
        {

        }

        public BusLineStation(BusLineStation other)
            : base(other.BusStationKey, other.X, other.Y)
        {
            DistanceFromPrevStation = other.DistanceFromPrevStation;
            TimeFromPrevStation = other.TimeFromPrevStation;
        }
        /// <summary>
        /// get the distansce between tow stations
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double DistanceBetweenStations(BusLineStation other)
        {
            return this.DistanceBetweenCoord(new Coordinate(other.X, other.Y));
        }

        /// <summary>
        /// compare by the key
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if the keys ar equals else false</returns>
        public bool Equals(BusLineStation other)
        {
            return this.BusStationKey == other.BusStationKey;
        }

    }
}
