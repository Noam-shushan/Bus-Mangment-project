using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Bus
    {
        public int LicenseNum { get; set; }
        public DateTime FromDate { get; set; }
        public double TotalTrip { get; set; }
        public double FuelRemain { get; set; }
        public BusStatus Status { get; set; }
        public double KilometersAfterFueling { get; set; }
        public double KilometersAfterTreatment { get; set; }
        public DateTime LastTreatment { get; set; }
        public bool IsDeleted { get; set; }

        public const int KILOMETER_BEFORE_TREATMENT = 20000;
        public const int MAX_KILOMETER_AFTER_REFUELING = 1200;
        public const int FULL_CONTAINER = 500;
    }
}
