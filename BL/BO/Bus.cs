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
        public int KilometersAfterFueling { get; set; }
        public int KilometersAfterTreatment { get; set; }
        public bool IsDeleted { get; set; }
    }
}
