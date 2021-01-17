using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DS;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;

namespace Test
{
    public static class Program
    {
        static string linesPath = @"LinesXml.xml";
        static string adjacentStationsPath = @"AdjacentStationsXml.xml";
        static string lineStationsPath = @"LineStationsXml.xml";
        static string stationPath = @"StationsXml.xml";
        static string bussPath = @"BussXml.xml";
        static string usersPath = @"UserXml.xml";

        public static XElement CreateElement<T>(T obj)
        {
            var res = new XElement(typeof(T).Name);
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                res.Add(new XElement(prop.Name, prop.GetValue(obj)));
            }
            return res;
        } 

        static void creatXmls<T>(List<T> list, string path, string name)
        {
            XElement root = new XElement(name);
            foreach (var item in list)
            {
                root.Add(CreateElement(item));
            }
            root.Save(path);
        }

        static void Main(string[] args)
        {
            // SaveListToXMLSerializer(DataSource.LinesList, linesPath);
            // SaveListToXMLSerializer(DataSource.AdjacentStationsList, adjacentStationsPath);
            // SaveListToXMLSerializer(DataSource.LineStationsList, lineStationsPath);
            //SaveListToXMLSerializer(DataSource.StationsList, stationPath);
            //creatXmls(DataSource.BussList, bussPath, "Buss");
            //SaveListToXMLSerializer(DataSource.UsersList, usersPath);
            var b = new DO.Bus { LicenseNum = 6418226, FromDate = DateTime.Now };
            UpdateBus(b);
            
            Console.ReadKey();
        }

        public static void UpdateBus(DO.Bus bus)
        {
            XElement bussRootElem = LoadListFromXMLElement(bussPath);

            var busNode = (from b in bussRootElem.Elements()
                           where b.Element("LicenseNum").Value == bus.LicenseNum.ToString()
                           select b).FirstOrDefault();

            if (busNode == null)
                throw new DO.BadBusException(0, "bus not found");
            if (busNode.Element("IsDeleted").Value == true.ToString())
                throw new DO.BadBusException(bus.LicenseNum, "bus not found");

            UpdateElement(busNode, bus);
            SaveListToXMLElement(bussRootElem, bussPath);
        }

        public static void UpdateElement<T>(this XElement element, T obj)
        {
            foreach (var elem in element.Elements())
            {
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (prop.Name == elem.Name)
                        elem.SetValue(prop.GetValue(obj));
                }
            }
        }

        public static void RemoveBus(DO.Bus bus)
        {
            XElement bussRootElem = LoadListFromXMLElement(bussPath);

            var busNode = (from b in bussRootElem.Elements()
                           where b.Element("LicenseNum").Value == bus.LicenseNum.ToString()
                           select b).FirstOrDefault();

            if (busNode == null)
                throw new DO.BadBusException(0, "bus not found");
            if (busNode.Element("IsDeleted").Value == true.ToString())
                throw new DO.BadBusException(bus.LicenseNum, "bus not found");

            busNode.Element("IsDeleted").SetValue(true);
            //bussRootElem.Add(busNode);
            SaveListToXMLElement(bussRootElem, bussPath);
        }

        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return XElement.Load(filePath);
                }
                else
                {
                    XElement rootElem = new XElement(filePath);
                    rootElem.Save(filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.XmlFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }

        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.XmlFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }

        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XmlFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XmlFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static DO.Bus GetBus(int licenseNum)
        {
            XElement bussRootElem = LoadListFromXMLElement(bussPath);

            DO.Bus bus = (from b in bussRootElem.Elements()
                          where int.Parse(b.Element("LicenseNum").Value) == licenseNum
                          select new DO.Bus()
                          {
                              LicenseNum = int.Parse(b.Element("LicenseNum").Value),
                              FromDate = DateTime.Parse(b.Element("FromDate").Value),
                              FuelRemain = double.Parse(b.Element("FuelRemain").Value),
                              IsDeleted = bool.Parse(b.Element("IsDeleted").Value),
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

        public static void AddBus(DO.Bus bus)
        {
            var rootElem = LoadListFromXMLElement(bussPath);
            var temp = (from b in rootElem.Elements()
                        where b.Element("LicenseNum").Value == bus.LicenseNum.ToString()
                        select b).FirstOrDefault();
            if (temp != null)
            {
                if (!bool.Parse(temp.Element("IsDeleted").Value))
                    throw new DO.BadBusException(bus.LicenseNum, "Duplicate License number of bus");
            }
            
            rootElem.Add(CreateElement(bus));
            SaveListToXMLElement(rootElem, bussPath);
        }
    }
}
