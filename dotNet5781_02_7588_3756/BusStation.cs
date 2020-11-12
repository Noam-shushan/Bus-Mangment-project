using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//file 1
namespace dotNet5781_02_7588_3756
{
    /// <summary>
    /// class Coordinate
    /// </summary>
    public class Coordinate
    {

        private double _x;
        private double _y;

        /// <summary>
        /// //latitude in range [-90, 90]
        /// </summary>
        public double X 
        {
            get => _x;
            set
            {
                if (value <= 90 && value >= -90) 
                    _x = value;
            }
        }
        /// <summary>
        /// longitude in range [-180, 180]
        /// </summary>
        public double Y
        {
            get => _y;
            set
            {
                if (value <= 180 && value >= -180)
                    _y = value;
            }
        }

        public Coordinate(double x, double y) 
        {
            X = x; 
            Y = y; 
        }

        public double DistanceBetweenCoord(Coordinate other)// result >= 0
        {
            double distX = X - other.X;
            double distY = Y - other.Y;
            return Math.Sqrt(distX*distX + distY*distY);
        }
    }

    public class BusStation : Coordinate
    {
        public string BusStationKey { get; set; } // min 6 digits
        public string AddresBusStation { get; set; }

        public BusStation(string key, double latitude, double longitude) 
            : base(latitude, longitude)
        {
            BusStationKey = key;
        }


        public override string ToString() // Bus Station Code: 765432, 31.234567°N 34.56789°E
        {
            return $"Bus Station Code: {BusStationKey}, {X}°N {Y}°E";
        }
    }
}
