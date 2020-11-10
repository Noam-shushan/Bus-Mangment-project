using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7588_3756
{
    class BusLineStation : BusStation
    {
        private double _distanceFromPrevStation;
        TimeSpan _timeSpanFromPrevStation;

        public BusLineStation(BusStation prevStation, int key, float latitude, float longitude) 
            : base(key, latitude,longitude)
        {
            // צריך כאן את התחנה הקודמת
        }

        public double DistanceBetweenStations(BusLineStation other)
        {
            return this.DistanceBetweenCoord(new Coordinate(other.X, other.Y));
        }

        TimeSpan? TimeBetweenStations(BusLineStation other)
        {
            return null; // תממש את זה
        }
        
        /// <summary>
        /// compare by the key
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if the keys ar equals else false</returns>
        public bool Equals(BusLineStation other)
        {
            return this.BusStationKey == other.BusStationKey;
        }

    }
    
    //// אני חושב שכדאי אולי לשמור את כל התחנות במבנה נפרד  
    //public class StationList
    //{
    //    private List<BusLineStation> _allBusStationList;

    //    public StationList()
    //    {
    //        _allBusStationList = new List<BusLineStation>();
    //    }

    //    public bool AddToList(BusLineStation bls)
    //    {
    //        if (!_allBusStationList.Any())
    //        {
    //            _allBusStationList.Add(bls);
    //            return true;
    //        }
             
    //        foreach(var s in _allBusStationList)
    //        {
    //            if (s.Equals(bls))
    //                return false;
    //        }
    //        _allBusStationList.Add(bls);
    //        return true;
    //    }
    //}
}
