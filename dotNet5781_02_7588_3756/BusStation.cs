using System;

namespace dotNet5781_02_7588_3756
{
    /// <summary>
    /// class Coordinate
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// //latitude in range [-90, 90]
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// longitude in range [-180, 180]
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Coordinate(double x, double y) 
        {
            X = x; 
            Y = y; 
        }
        /// <summary>
        /// get the distance between two coordinate
        /// </summary>
        /// <param name="other"></param>
        /// <returns>the distance between two coordinate </returns>
        public double DistanceBetweenCoord(Coordinate other)// result >= 0
        {
            double distX = X - other.X;
            double distY = Y - other.Y;
            return Math.Sqrt(distX*distX + distY*distY);
        }
    }
    /// <summary>
    /// bus station class, Inherits the coordinate class
    /// </summary>
    public class BusStation
    {
        public int BusStationKey { get; set; } // min 6 digits
        public string AddresBusStation { get; set; }
        public Coordinate MyLocation { get; set; }
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public BusStation(int key, double latitude, double longitude) 
        {
            BusStationKey = key;
            MyLocation = new Coordinate(latitude, longitude);
        }
        /// <summary>
        /// print the location if the station and the key number
        /// for example "Bus Station Code: 765432, 31.234567°N 34.56789°E"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Bus Station Code: {BusStationKey}, {MyLocation.X}°N {MyLocation.Y}°E";
        }
    }
}
