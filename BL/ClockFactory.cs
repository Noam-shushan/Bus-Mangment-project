using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public class ClockFactory
    {
        public static BL.Clock GetClock()
        {
            return BL.Clock.Instance;
        }
    }
}