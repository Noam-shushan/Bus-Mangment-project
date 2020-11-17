using System;
using System.Linq;

namespace dotNet5781_01_7588_3756
{
    class Program
    {
        static void Main(string[] args)
        {
            //BusList myBusList = new BusList();
            //UserInput.Menu(myBusList); 
            Random r = new Random();
            TimeSpan[] tArr = new TimeSpan[5];
            TimeSpan t = new TimeSpan();
            for (int i = 0; i < tArr.Length; i++)
            {
                tArr[i] = new TimeSpan(0, r.Next(0, 60), 0);
                t = t.Add(tArr[i]);
            }
            for (int i = 0; i < tArr.Length; i++)
            {
                Console.WriteLine(tArr[i]);
            }
            Console.WriteLine($"total = {t}");
            Console.ReadLine();
        }
    }    
}
