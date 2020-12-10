using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03B_7588_3756
{
    class MyData
    {
        public Random rand = new Random();
        public ObservableCollection<MyBus> MyBusList { get; } = new ObservableCollection<MyBus>(); // 
        public static List<int> UniqueLicenseNumbers { get; } = new List<int>(); // 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numOfBuss"></param>
        public void InitializeBusList(int numOfBuss)
        {
            for (int i = 0; i < numOfBuss; i++)
            {
                var startActivity = randomDate();
                string licenseNumber = randomLicenseNumber(startActivity);
                var lastTreatment = randomDate(startActivity.Year);
                int kilometers = rand.Next(2000, 100000);
                int kilometersAfterTreatment = rand.Next(0, Bus.KILOMETER_BEFORE_TREATMENT);
                MyBusList.Add(new MyBus(licenseNumber, startActivity,
                    kilometers, lastTreatment, kilometersAfterTreatment));
            }
            MyBusList[0].Treatment();
            MyBusList[1].KilometersAfterTreatment = Bus.KILOMETER_BEFORE_TREATMENT + 1;
            MyBusList[2].KilometersAfterFueling = Bus.MAX_KILOMETER_AFTER_REFUELING + 1;
        }

        private DateTime randomDate(int year = 2000)
        {
            DateTime start = new DateTime(year, 1, 1); 
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rand.Next(range));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="licenseNumber"></param>
        /// <returns></returns>
        public static bool CheckExistingLicenseNumber(int licenseNumber)
        {
            return UniqueLicenseNumbers.Contains(licenseNumber);
        }

        private int getUniqueLicenseNumber(int max)
        {
            int res = rand.Next(max);
            while (UniqueLicenseNumbers.Contains(res))
                res = rand.Next(max);
            UniqueLicenseNumbers.Add(res);
            return res;
        }

        private string randomLicenseNumber(DateTime startActivity)
        {
            string res;
            if (startActivity.Year < 2018)
            {
                res = getUniqueLicenseNumber(10000000).ToString();
                int numOfZeros = Math.Abs(res.Length - 7);
                for (int j = 0; j < numOfZeros; j++)
                    res = res.Insert(0, "0");
            }
            else
            {
                res = getUniqueLicenseNumber(100000000).ToString();
                int numOfZeros = Math.Abs(res.Length - 8);
                for (int j = 0; j < numOfZeros; j++)
                    res = res.Insert(0, "0");
            }
            return res;
        }
    }
}
