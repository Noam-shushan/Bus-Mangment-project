using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7588_3756
{
    class StationList
    {
        public List<BusLineStation> AllBusStations { get; set; }

        public StationList()
        {
            AllBusStations = new List<BusLineStation>();
        }  

        public bool Insert(BusLineStation bls)
        {
            if (IsEmpty())
            {
                AllBusStations.Add(bls);
                return true;
            }

            if (IsStationExists(bls))
                return false;
            
            AllBusStations.Add(bls);
            return true;
        }

        public void Remove(BusLineStation bls)
        {
            AllBusStations.Remove(bls);
        }

        public bool IsEmpty()
        {
            return !AllBusStations.Any();
        }

        public bool IsStationExists(BusLineStation bls)
        {
            foreach (var s in AllBusStations)
            {
                if (s.Equals(bls))
                    return true;
            }
            return false;
        }
    }
}
