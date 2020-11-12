using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7588_3756
{
    class BusLineCollection : IEnumerable<BusLine> 
    {
        public List<BusLine> BusLineList { get; set; }
        
        public BusLineCollection()
        {
            BusLineList = new List<BusLine>();
        }

        public bool AddBusLine(BusLine newBusLine)
        {
            if (!validInsert(newBusLine))
                return false;

            BusLineList.Add(newBusLine);
            return true;
        }

        public void RemoveBusLine(BusLine busToDel)
        {
            BusLineList.Remove(busToDel);
        }

        public List<BusLine> GetListPassInStation(string stationKey)
        {
            bool found = false;
            List<BusLine> output = new List<BusLine>();
            foreach (var bl in BusLineList)
            {
                foreach(var st in bl.Stations)
                {
                    if(st.BusStationKey == stationKey)
                    {
                        found = true;
                        output.Add(bl);
                    }
                }
            }
            
            if (!found)
                return null;
            
            return output;
        }

        public List<BusLine> SortBusLineList()
        {
            var sortedList = BusLineList.ToList();
            sortedList.Sort();
            return sortedList;
        }

        public BusLine this[int i]
        {
            get => BusLineList.ElementAt(i); 
            set => BusLineList.Insert(i, value);
        }

        private bool validInsert(BusLine bl)
        {
            int found = 0;
            BusLine sameLine1 = null, sameLine2 = null;
            foreach (var item in BusLineList)
            {
                if (item.Equals(bl))
                {
                    found++;
                }
                if(found == 1)
                    sameLine1 = new BusLine(item);
                if(found == 2)
                    sameLine2 = new BusLine(item);
                if (found > 2)
                    return false;
            }
            if(found == 2)
            {
                if (sameLine1.FirstStation.Equals(sameLine2.LastStation) &&
                    sameLine1.LastStation.Equals(sameLine2.FirstStation))
                    return true;
            }
            if (found == 1)
                return true;
            return false;
        }
        
        public IEnumerator<BusLine> GetEnumerator()
        {
            return BusLineList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
