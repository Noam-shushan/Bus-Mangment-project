using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_7588_3756
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> l = new List<int>{ 1, 6 };
            l.Insert(1, 2);
            l.Insert(1, 4);
            l.Insert(1, 5);
            l.ForEach(Console.WriteLine);
            Console.ReadKey();

        }
    }
}
