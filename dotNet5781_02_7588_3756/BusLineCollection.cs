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
            if (!ValidInsert(newBusLine))
                return false;
            
            BusLineList.Add(newBusLine);
            return true;
        }

        public bool ValidInsert(BusLine bl)
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
                    sameLine1 = item;
                if(found == 2)
                    sameLine2 = item;
                if (found > 2)
                    return false;
            }
            if(found == 2)
            {
                if (sameLine1.FirstStation.Equals(sameLine2.LastStation) &&
                    sameLine1.LastStation.Equals(sameLine2.FirstStation))
                    return true;
            }
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
