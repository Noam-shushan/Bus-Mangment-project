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
using System.Security.Cryptography;

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

        private static string hashPassword(string password)
        {
            SHA512 shaM = new SHA512Managed();
            return Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private static string readPassword()
        {
            Console.WriteLine("Please enter password");
            return Console.ReadLine();
        }

        public class User
        {
            public int Id { get; set; }
            public string HashedPassword { get; set; }
        }
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            SaveListToXMLSerializer(DataSource.LinesList, linesPath);
            SaveListToXMLSerializer(DataSource.AdjacentStationsList, adjacentStationsPath);
            SaveListToXMLSerializer(DataSource.LineStationsList, lineStationsPath);
            SaveListToXMLSerializer(DataSource.StationsList, stationPath);
            creatXmls(DataSource.BussList, bussPath, "Buss");
            SaveListToXMLSerializer(DataSource.UsersList, usersPath);
            SaveListToXMLSerializer(DataSource.LineTripsList, @"LinesTripXml.xml");
            List<int> CountersList = new List<int>
            {
                DataSource.LinesList.Last().Id,
                DataSource.LineTripsList.Last().Id
            };
            SaveListToXMLSerializer(CountersList, @"CountersXml.xml");
            Console.ReadKey();
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
    }
}
