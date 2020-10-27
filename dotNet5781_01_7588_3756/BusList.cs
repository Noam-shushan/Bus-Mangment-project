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
                Console.WriteLine(@"Insert option:");
                Console.WriteLine(@"    1 - Insert bus ");
                Console.WriteLine(@"    2 - Get bus ");
                Console.WriteLine(@"    3 - Bus service");
                Console.WriteLine(@"    0 - Exit ");
                Console.Write(@"Your option :   ");

                option = Console.ReadLine();
                this.checkMenuOption(option);
            } while (option != "0");
        }

        public int GetLiscenseNumberFromUser()
        {
            int result;
            Console.Write(@"Insert licensing number of the bus: ");
            string liscenseNumber = Console.ReadLine();
            if (!int.TryParse(liscenseNumber, out result))
            {
                Console.WriteLine("Erorr: invalid liscense number");
                return -1;
            }
            return result;
        }

        public void checkMenuOption(string option)
        {
            //Console.Clear();
            Console.WriteLine(@"********");
            switch (option)
            {
                case "1":
                    AddBusToList();
                    break;
                case "2":
                    ChooseBus();
                    break;
                case "3": break;
                case "0": break;
                default:
                    Console.WriteLine(@"Error Invalid option");
                    break;
            }
            Console.WriteLine(@"********");

        }

        private void AddBusToList()
        {
            int liscenseNumber = this.GetLiscenseNumberFromUser();
            Console.Write(@"Insert a start activity date of the bus '(yyyy-mm-dd)': ");
            string startActivity = Console.ReadLine();

            Bus newBus = new Bus(0, Convert.ToDateTime(startActivity));

            if (!newBus.ValidInput(liscenseNumber, startActivity))
            {
                Console.WriteLine("Invalid licence number");
                return;
            }
            newBus.LiscenseNumber = liscenseNumber;
            this.InsertBus(newBus);
            Console.WriteLine(@"Bus inserted");
        }

        private void ChooseBus()
        {
            int liscenseNumber = this.GetLiscenseNumberFromUser();
            Bus selectedBus = this.getBus(liscenseNumber);
            
            if (!this.ExiestBus(selectedBus))
                return;
            
            if (selectedBus.NeedsTreatment())
            {
                Console.Write("This bus need treatment");
                return;                     
            }
            
            Random rand = new Random(DateTime.Now.Millisecond);
            int kilometers = rand.Next();
            
            if (selectedBus.NeedRefueling(kilometers))
            {
                Console.Write(@"This bus need refueing to make this drive");
                return;
            }
            
            selectedBus.Kilometers = kilometers;
            Console.WriteLine("the drive approved");
        }
        
        public void GetService()
        {
            int liscenseNumber = this.GetLiscenseNumberFromUser();
            
            Console.Write("enter:\n" +
                "1 - for fueling the bus:\n" +
                "2 - to make treatment");
            
            string option = Console.ReadLine();
            Bus selectedBus = this.getBus(liscenseNumber);
            
            if(!this.ExiestBus(selectedBus)) 
                return;
            
            switch (option)
            {
                case "1":
                    selectedBus.IsFueled = true;
                    Console.WriteLine("bus {0} get Fueled", liscenseNumber);
                    break;
                case "2":
                    selectedBus.Treatment();
                    Console.WriteLine("bus {0} Treated", liscenseNumber);
                    break;
            }

        }

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
