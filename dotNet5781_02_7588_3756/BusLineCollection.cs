using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace dotNet5781_02_7588_3756
{
    /// <summary>
    /// class BusLineCollection, implamnt IEnumerable and Indexer
    /// </summary>
    public class BusLineCollection : IEnumerable<BusLine> 
    {
        public List<BusLine> BusLineList { get; set; }
        /// <summary>
        /// constractor
        /// </summary>
        public BusLineCollection()
        {
            BusLineList = new List<BusLine>();
        }
        /// <summary>
        /// add bus to the collection
        /// </summary>
        /// <param name="newBusLine"></param>
        /// <returns></returns>
        public bool AddBusLine(BusLine newBusLine)
        {
            if (!validInsert(newBusLine))
                return false;

            BusLineList.Add(newBusLine);
            return true;
        }
        /// <summary>
        /// remove bus from the collection
        /// </summary>
        /// <param name="busToDel"></param>
        public void RemoveBusLine(BusLine busToDel)
        {
            BusLineList.Remove(busToDel);
        }
        /// <summary>
        /// get list of the buss the pass in the given station
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns>List of BusLine or null if not found any</returns>
        public List<BusLine> GetListPassInStation(int stationKey)
        {
            bool found = false;
            List<BusLine> output = new List<BusLine>();
            foreach (var bl in BusLineList)
            {
                foreach (var st in bl.Stations)
                { 
                    if (st.BusStationKey == stationKey)
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
        /// <summary>
        /// sort the collection by the short total time of the lines
        /// </summary>
        /// <returns>sorted list of BusLine</returns>
        public List<BusLine> SortBusLineList()
        {
            var sortedList = BusLineList.ToList();
            sortedList.Sort();
            return sortedList;
        }
        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="i"></param>
        /// <returns>the BusLine that have the bus line number i</returns>
        public BusLine this[int i]
        {
            get => BusLineList.Find(x => x.BusLineNum == i); 
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
            if(found == 2)// check if the departure and destination stations are reversed
            {
                if (sameLine1.FirstStation.Equals(sameLine2.LastStation) &&
                    sameLine1.LastStation.Equals(sameLine2.FirstStation))
                    return true;
                else
                    return false;
            }
            return true;
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
