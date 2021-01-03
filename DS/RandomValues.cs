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
            latitude = random.NextDouble() + random.Next(33, 36);
            longitude = random.NextDouble() + random.Next(29, 34);
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
    }
}
