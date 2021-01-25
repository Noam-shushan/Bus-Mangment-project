using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// 
    /// </summary>
    public class LineTrip
    {
        public int Id { get; set; }
        public int LineId { get; set; }
        public int StartAtInMinutes { get; set; }
        public int StartAtInHours { get; set; }
    }
}
