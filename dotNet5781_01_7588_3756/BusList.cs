using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_7588_3756
{
    class BusList
    {
        private List<Bus> _allBus;
        
        public BusList()
        {
            _allBus = new List<Bus>();
        }

        

        public void InsertBus(Bus newBus) 
        {
            _allBus.Add(newBus);
        }

        public Bus getBus(int licenseNumber)
        {
            foreach (Bus b in _allBus)
            {
                if (licenseNumber == b.LiscenseNumber)
                    return b;
            }
            return null;
        }

        public void menu()
        {
            string option = "";
            do
            {
                Console.WriteLine(@"Insert option: ");
                Console.WriteLine(@"    1 - Insert bus ");
                Console.WriteLine(@"    2 - Get bus ");
                Console.WriteLine(@"    3 - Bus service ");
                Console.WriteLine(@"    0 - Exit ");
                Console.Write(@"Your option :   ");

                option = Console.ReadLine();
                this.checkMenuOption(option);
            } while (option != "0");
        }

        public void checkMenuOption(String option)
        {
            string liscenseNumber = "";
            //Console.Clear();
            Console.WriteLine(@"********");
            switch (option)
            {
                case "1":
                    AddBusToList(liscenseNumber);
                    break;
                case "2":
                    ChooseBus(liscenseNumber);
                    break;
                case "3": break;
                case "0": break;
                default:
                    Console.WriteLine(@"Error Invalid option");
                    break;
            }
            Console.WriteLine(@"********");

        }

        private void AddBusToList(string liscenseNumber)
        {
            Console.Write(@"Insert a start activity date of the bus '(yyyy-mm-dd)': ");
            string startActivity = Console.ReadLine();

            Bus newBus = new Bus(0, Convert.ToDateTime(startActivity));

            Console.Write(@"Insert licensing number of the bus: ");
            liscenseNumber = Console.ReadLine();
            while (!newBus.ValidInput(liscenseNumber, startActivity))
            {
                Console.WriteLine("Invalid licence number, enter agein: ");
                liscenseNumber = Console.ReadLine();
            }
            newBus.LiscenseNumber = int.Parse(liscenseNumber);
            this.InsertBus(newBus);
            Console.WriteLine(@"Bus inserted");
        }

        private void ChooseBus(string liscenseNumber)
        {
            Console.Write(@"Insert licensing number of the bus: ");
            liscenseNumber = Console.ReadLine();
            Bus selectedBus = this.getBus(int.Parse(liscenseNumber));
            if (selectedBus.NeedsTreatment())
            {
                Console.Write(@"This bus need treatment\n
                            do you wont to make a treatment? (y/n): ");
                if (Console.ReadLine() == "y")
                    selectedBus.Treatment();
                else
                {
                    Console.Write(@"This bus can not travel");
                    return;
                }        
            }
            Random rand = new Random(DateTime.Now.Millisecond);
            int kilometers = rand.Next();
            if (selectedBus.NeedRefueling(kilometers))
            {
                Console.Write(@"The bus need refueing to make this drive\n
                            do you wont to refuil? (y/n): ");
                selectedBus.IsFueled = Console.ReadLine() == "y" ? true : false;
            }
            selectedBus.Mileage = kilometers;
        }

    }
}
