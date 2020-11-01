using System;
using System.Linq;

namespace dotNet5781_01_7588_3756
{
    static class UserInput
    {
        private const int DIGITS_FOR_BUS_BEFORE_2018 = 7;
        private const int DIGITS_FOR_BUS_AFTER_2018 = 8;

        /*
         * menu selction for the user
         */
        public static void Menu(BusList myBusList)
        {
            string option = "";
            do
            {
                Console.WriteLine(@"Insert option:");
                Console.WriteLine(@"    1 - Insert bus to the bus list");
                Console.WriteLine(@"    2 - Choose a bus to travel");
                Console.WriteLine(@"    3 - Bus service (Refueling/Treatment)");
                Console.WriteLine(@"    4 - View bus data");
                Console.WriteLine(@"    0 - Exit ");
                Console.Write(@"Your option :   ");

                option = Console.ReadLine();
                CheckMenuOption(option, myBusList);
            } while (option != "0");
        }
        /*
         * Check the selection by the user and apply his request
         */
        public static void CheckMenuOption(string option, BusList myBusList)
        {
            Console.WriteLine("********");
            switch (option)
            {
                case "1":
                    myBusList.AddBusToList();
                    break;
                case "2":
                    myBusList.ChooseBus();
                    break;
                case "3":
                    myBusList.GetService();
                    break;
                case "4":
                    myBusList.PrintBusList();
                    break;
                case "0":
                    Console.WriteLine("\nExit program");
                    break;
                default:
                    Console.WriteLine("\nError: Invalid option");
                    break;
            }
            Console.WriteLine("\n********");
        }
        /*
         * get the start activity from the user by string
         * return DateTime object with the user input
         */
        public static DateTime? GetStartActivityFromUser()
        {
            Console.Write(@"Insert a start activity date of the bus '(yyyy-mm-dd)': ");
            DateTime startActivity;
            
            if(!DateTime.TryParse(Console.ReadLine(), out startActivity))
            {
                Console.WriteLine("Error: invalid date time");
                return null;
            }
            
            return startActivity;
        }
        /*
         * get liscense number from the user
         * Check if it is valid in relation to the date of
         * stert of the bus activity
         * @param - startActivity: the date of start activity
         * @return - the user input if is valid liscense number 
         *          else empty string
         */
        public static string GetLiscenseNumberFromUser(DateTime? startActivity)
        {
            Console.Write(@"Insert liscense number of the bus: ");
            string liscenseNumber = Console.ReadLine();
            
            if(!validLiscenseNumber(liscenseNumber.Replace("-", ""), startActivity) ||
                !formatValidLiscenseNumber(liscenseNumber))
            {
                Console.WriteLine("\nError: Invalid liscense number");
                return "";
            }
            
            return liscenseNumber.Replace("-", "");
        }
        /*override the previs function
         * gest return the user input for the liscense number
         */
        public static string GetLiscenseNumberFromUser()
        {
            Console.Write(@"Insert liscense number of the bus: ");
            string liscenseNumber = Console.ReadLine();
            if (!formatValidLiscenseNumber(liscenseNumber))
            {
                Console.WriteLine("Error: Invalid liscense number");
                return "";
            }
            return liscenseNumber.Replace("-", "");
        }
        /*
         * check valid liscense number 
         * regarding the date of commencement of its activity
         */
        private static bool validLiscenseNumber(string liscenseNumber, DateTime? startActivity)
        {
            if (startActivity?.Year < 2018 &&
                    liscenseNumber.Length == DIGITS_FOR_BUS_BEFORE_2018)
                return true;
            if (startActivity?.Year >= 2018 &&
                liscenseNumber.Length == DIGITS_FOR_BUS_AFTER_2018)
                return true;
            return false;
        }
        /*
         * check if the liscense number the input from the user
         * is contain only "123456789-"
         */
        private static bool formatValidLiscenseNumber(string liscenseNumber)
        {
            string allowableLetters = "123456789-";

            foreach (char c in liscenseNumber)
            {
                if (!allowableLetters.Contains(c.ToString()))
                    return false;
            }
            // if there is more then 2 '-' in the input
            // or it star or end with '-'
            if (liscenseNumber.Count(c => c == '-') > 2 ||
                liscenseNumber.StartsWith("-") || 
                liscenseNumber.EndsWith("-"))
                return false;

            return true;
        }

    }
}
