using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    static class RandomValues
    {
        static List<int> randomListStationKeys = new List<int>();
        static List<int> uniqueLicenseNumbers = new List<int>();
        static Random random = new Random(1000);
        static int lineCounter = 0;

        internal static int getLineCounter()
        {
            lineCounter++;
            return lineCounter;
        }

        internal static int getUniqueStationKey()
        {
            int key = random.Next(100000, 999999);
            while (randomListStationKeys.Contains(key))
            {
                key = random.Next(100000, 999999);
                randomListStationKeys.Add(key);
            }
            return key;
        }

        internal static void getLocation(out double latitude, out double longitude)
        {
            double xTemp = random.NextDouble();
            double yTemp = random.NextDouble();
            xTemp += random.Next(33, 36);
            yTemp += random.Next(29, 34);
            latitude = xTemp;
            longitude = yTemp;
        }

        internal static int getLineCode()
        {
            return random.Next(1, 999);
        }

        internal static int getStation(List<DO.LineStation> uniqueStatinCode,
            DO.Areas areaOfFirstStation = DO.Areas.GENERAL)
        {
            if(uniqueStatinCode.Count == 0)
                return DataSource.StationsList.ElementAt(random.Next(0, DataSource.StationsList.Count)).Code;
            
            var lTemp = DataSource.StationsList.Where(s1 => 
                uniqueStatinCode.All(s2 => s1.Code != s2.Station));
            lTemp = from s in lTemp
                    where s.Area == areaOfFirstStation
                    select s;
            return lTemp.ElementAt(random.Next(0, lTemp.Count())).Code;
        }

        internal static DateTime randomDate(int year = 2000)
        {
            DateTime start = new DateTime(year, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }


        static int getUniqueLicenseNumber(int max)
        {
            int res = random.Next(max/10 ,max);
            while (uniqueLicenseNumbers.Contains(res))
                res = random.Next(max/10 ,max);
            uniqueLicenseNumbers.Add(res);
            return res;
        }

        internal static int randomLicenseNumber(DateTime startActivity)
        {
            if (startActivity.Year < 2018)
                return getUniqueLicenseNumber(10000000);
            return getUniqueLicenseNumber(100000000);
        }

        internal static double getKilometers()
        {
            return random.Next(2000, 100000);
        }

        internal static double getFuelLiters()
        {
            return random.Next(500);
        }

        static bool inRange(double param1, double param2, double x1, double y1, double x2, double y2)
        {
            return param1 >= x1 && param1 <= y1 && param2 >= x2 && param2 <= y2;
        }

        internal static DO.Areas getArea(double latitude, double longitude)
        {
            if (inRange(latitude, longitude, 34.461262, 35.2408, 31.83538, 32.26356))
                return DO.Areas.CENTER;

            if (inRange(latitude, longitude, 35.1252, 35.2642, 31.7082, 31.8830))
                return DO.Areas.JERUSALEM;

            if (inRange(latitude, longitude, 34.8288611, 35.97865, 32.3508222, 33.3579972))
                return DO.Areas.NORTH;

            if (inRange(latitude, longitude, 33.81264994, 35.5857, 29.47925, 30.493059))
                return DO.Areas.SOUTH;

            return DO.Areas.GENERAL;
        }
    }
}
