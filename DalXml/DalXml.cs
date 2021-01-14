using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    public class DalXml 
    {
        #region singelton
        //public static DalApi.IDaL Instance { get; } = new DalXml();
        
        DalXml() { }
        #endregion 

        string bussPath = "";

        public DO.Bus GetBus(int licenseNum)
        {
            XElement bussRootElem = XmlTools.LoadListFromXMLElement(bussPath);

            DO.Bus bus = (from b in bussRootElem.Elements()
                        where int.Parse(b.Element("LicenseNum").Value) == licenseNum
                        select new DO.Bus()
                        {
                            LicenseNum = int.Parse(b.Element("LicenseNum").Value),
                            FromDate = DateTime.Parse(b.Element("FromDate").Value),
                            FuelRemain = double.Parse(b.Element("FuelRemain").Value),
                            IsDeleted  = bool.Parse(b.Element("IsDeleted").Value),
                            KilometersAfterFueling = double.Parse(b.Element("KilometersAfterFueling").Value),
                            KilometersAfterTreatment = double.Parse(b.Element("KilometersAfterTreatment").Value),
                            LastTreatment = DateTime.Parse(b.Element("LastTreatment").Value),
                            Status = (DO.BusStatus)Enum.Parse(typeof(DO.BusStatus), b.Element("Status").Value),
                            TotalTrip = double.Parse(b.Element("TotalTrip").Value)
                        }
                        ).FirstOrDefault();

            if (bus != null && !bus.IsDeleted)
                return bus;
            else
                throw new DO.BadBusException(licenseNum, "bus not found");
        }
    }
}
