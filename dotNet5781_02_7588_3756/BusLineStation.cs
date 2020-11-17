using System;


namespace dotNet5781_02_7588_3756
{
    /// <summary>
    /// class bus line station, Inherits the BusStation class
    /// </summary>
    public class BusLineStation : BusStation
    {
        public double DistanceFromPrevStation { get; set; }
        public TimeSpan? TimeFromPrevStation { get; set; }
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public BusLineStation(int key, double latitude, double longitude)
            : base(key, latitude, longitude)
        {

        }
        /// <summary>
        /// copy constractor
        /// </summary>
        /// <param name="other"></param>
        public BusLineStation(BusLineStation other)
            : base(other.BusStationKey, other.MyLocation.X, other.MyLocation.Y)
        {

        }
        /// <summary>
        /// get the distansce between tow stations
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double DistanceBetweenStations(BusLineStation other)
        {
            return this.MyLocation.DistanceBetweenCoord(new Coordinate(other.MyLocation.X, other.MyLocation.Y));
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

        public override string ToString()
        {
            return $"Bus Station Code: {BusStationKey}, {MyLocation.X}°N {MyLocation.Y}°E {TimeFromPrevStation}";
        }
    }
}
