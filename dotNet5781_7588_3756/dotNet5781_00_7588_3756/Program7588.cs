using System;


namespace dotNet5781_00_7588_3756
{
    partial class Program
    {
        static void Main(string[] args)
        {
            welcome7588();
            Console.ReadKey();
        }

        private static void welcome7588()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
