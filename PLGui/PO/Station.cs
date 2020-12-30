using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class Station
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsDeleted { get; set; }
        public BO.Areas Area { get; set; }
        public string Location 
        {
            get => $"{Latitude}°N {Longitude}°E";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"{Name}\n" +
            $"Station code: {Code}";
    }
}
