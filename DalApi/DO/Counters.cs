using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Counters
    {
        static int _lineCounter = 0;
        static int _busOnTripCounter = 0;
        static int _tripCounter = 0;

        public static int LineCounter
        {
            get
            {
                _lineCounter++;
                return _lineCounter;
            }
        }

        public static int BusOnTripCounter
        {
            get
            {
                _busOnTripCounter++;
                return _busOnTripCounter;
            }
        }

        public static int TripCounter
        {
            get
            {
                _tripCounter++;
                return _tripCounter;
            }
        }
    }
}
