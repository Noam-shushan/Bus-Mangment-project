using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public class Counters
    {
        static int _lineCounter = 0;
        static int _lineTripCounter = 0;

        public static int LineCounter
        {
            get
            {
                _lineCounter++;
                return _lineCounter;
            }
            set => _lineCounter = value;
        }

        public static int LineTripCounter
        {
            get
            {
                _lineTripCounter++;
                return _lineTripCounter;
            }
            set => _lineTripCounter = value;
        }
    }
}
