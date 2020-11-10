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

        private float _x;
        private float _y;

        /// <summary>
        /// //latitude range [-90, 90]
        /// </summary>
        public float X 
        {
            get => _x;
            set
            {
                if (value <= 90 && value >= -90) 
                    _x = value;
            }
        }
        /// <summary>
        /// longitude in ranga [-180, 180]
        /// </summary>
        public float Y
        {
            get => _y;
            set
            {
                if (value <= 180 && value >= -180)
                    _y = value;
            }
        }

        public Coordinate(float x, float y) 
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
        private int _busStationKey;
        public string AddresBusStation { get; set; }

        public BusStation(int key, float latitude, float longitude) 
            : base(latitude, longitude)
        {
            BusStationKey = key;
        }

        public int BusStationKey
        {
            get => _busStationKey;
            set
            {
                if (value > 99999) // min 6 digits
                    _busStationKey = value;
            }
        }

        public override string ToString() // Bus Station Code: 765432, 31.234567°N 34.56789°E
        {
            return ""; // תממש את זה
        } 





    }
}
