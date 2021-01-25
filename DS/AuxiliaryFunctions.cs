using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class AuxiliaryFunctions
    {
        static bool inRange(double param1, double param2, double x1, double x2, double y1, double y2)
        {
            return param1 >= x1 && param1 <= x2 && param2 >= y1 && param2 <= y2;
        }

        public static DO.Areas GetArea(double latitude, double longitude)
        {
            if (inRange(latitude, longitude, 34, 34.5, 29, 33))
                return DO.Areas.SOUTH;

            if (inRange(latitude, longitude, 34.5, 35, 29, 33))
                return DO.Areas.JERUSALEM;

            if (inRange(latitude, longitude, 35, 35.5, 29, 33))
                return DO.Areas.NORTH;

            if (inRange(latitude, longitude, 35.5, 36, 29, 33))
                return DO.Areas.CENTER;

            return DO.Areas.GENERAL;
        }

        public static double GetDisteance(double latitude1, double longitude1,
            double latitude2, double longitude2)
        {
            return new GeoCoordinate(latitude1, longitude1).GetDistanceTo(new
                GeoCoordinate(latitude2, longitude2));
        }

        public static TimeSpan GetTimeBetweenStations(double dist)
        {
            double km = dist / 1000;
            double kmPerMin = 0.5;
            double totalMinute = km / kmPerMin;
            return TimeSpan.FromMinutes(totalMinute);
        }

        public static string GetHashPassword(string password)
        {
            SHA512 shaM = new SHA512Managed();
            return Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
