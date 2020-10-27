using System;
using System.Collections.Generic;

namespace dotNet5781_01_7588_3756
{
    class BusList
    {
        private List<Bus> _allBus;
        /*
         * constractor
         */
        public BusList()
        {
            _allBus = new List<Bus>();
        }
        /*insert bus to the bus list
         */
        public void InsertBus(Bus newBus) 
        {
            _allBus.Add(newBus);
        }
        /*
         * get bus by liscense number from the bus list
         * @param - licenseNumber
         * @return - the bus object or null if not fuond in the list
         */
        public Bus GetBus(string licenseNumber)
        {
            foreach (Bus b in _allBus)
            {
                if (licenseNumber == b.LiscenseNumber)
                    return b;
            }
            return null;
        }
        /*
         * geting infromtion from the user to add new bus tothe list
         * add bus to the bus list
         */
        public void AddBusToList() // case "1"
        {
            DateTime? startActivity = UserInput.GetStartActivityFromUser();
            if (startActivity == null)
                return; // not valid Date

            string liscenseNumber = UserInput.GetLiscenseNumberFromUser(startActivity);
            if (liscenseNumber == "")
                return; // not valid license number

            Bus newBus = new Bus(liscenseNumber, startActivity);
            this.InsertBus(newBus);
            Console.WriteLine("bus inserted");
        }
        /*
         * Get infromtion about the bus
         * Sets travel distance randomly
         * apply the travel
         */
        public void ChooseBus() // case "2"
        {
            string liscenseNumber = UserInput.GetLiscenseNumberFromUser();
            Bus selectedBus = this.GetBus(liscenseNumber);            
            if (!this.ExiestBus(selectedBus))
                return; // bus not found in the list
            
            if (selectedBus.NeedsTreatment())
            {
                Console.WriteLine("Can not make the trip!");
                Console.Write("This bus need treatment");
                return; // Can not make a trip must take care of the bus
            }
            
            Random rand = new Random(DateTime.Now.Millisecond);
            int kilometers = rand.Next();
            Console.WriteLine("The travel distance is {0}", kilometers);
            
            if (selectedBus.NeedRefueling(kilometers) && !selectedBus.IsFueled)
            { // If both the new driving distance is greater than 1200
              //and the vehicle is not refueled
                Console.Write("This bus need refueing to make this drive");
                return;
            }
            
            selectedBus.Kilometers = kilometers;
            Console.WriteLine("the drive approved");
        }
        /*
         * Get infromtion about the bus
         * Performing refueling of the bus
         * Periodic treatment or treatment after 20,000 kilometers
         */
        public void GetService() // case "3"
        {
            string liscenseNumber = UserInput.GetLiscenseNumberFromUser();
            Bus selectedBus = this.GetBus(liscenseNumber);
            if (!this.ExiestBus(selectedBus))
                return; // bus not found in the bus list
            
            Console.Write("Insert option:\n" +
                "1 - for fueling the bus: \n" +
                "2 - to make treatment: ");            
            string option = Console.ReadLine();                    
            
            switch (option)
            {
                case "1":
                    selectedBus.IsFueled = true; // refueling
                    Console.WriteLine("bus get Fueled");
                    break;
                case "2":
                    selectedBus.Treatment(); // treatment
                    Console.WriteLine("bus Treated");
                    break;
            }
        }
        /*
         * print all bus liscense numbers,Kilometers and
         * date of last treatment
         */
        public void PrintBusList() // case "4"
        {
            foreach(Bus b in _allBus)
            {
                Console.WriteLine(b.toString() + "\n");
            }
        }
        /*
         * check if the bus exiest 
         */
        private bool ExiestBus(Bus b)
        {
            if (b == null)
            {
                Console.WriteLine("bus not fuond!");
                return false;
            }
            return true;
        }
    }
}
