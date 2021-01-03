using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Line
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public Areas Area { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<LineStation> LineStations { get; set; }
        public string FirstStationName
        {
            get => LineStations.ToList().First().Name;
        }
        public string LastStationName
        {
            get => LineStations.ToList().Last().Name;
        }
    }
}
