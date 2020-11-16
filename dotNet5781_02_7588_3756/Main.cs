using System;

namespace dotNet5781_02_7588_3756
{
    class Program
    {

        static void Main(string[] args)
        {
            BusLineCollection busColl = new BusLineCollection();
            InitializationAndMenu.InitializBusCollection(busColl, 10, 40);
            InitializationAndMenu.Menu(busColl);
        }
    }
}
