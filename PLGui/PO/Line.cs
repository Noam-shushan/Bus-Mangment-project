using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class Line
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public BO.Areas Area { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public IEnumerable<BO.LineStation> LineStations { get; set; }
        public bool IsDeleted { get; set; }
    }
}
