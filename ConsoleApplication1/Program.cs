using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace ConsoleApplication1
{

    class Program
    {
        static void Main(string[] args)
        {
            /*
            Random rnd = new Random();            

            using (TextWriter txt = new StreamWriter("days.txt"))
            {
                for (int i = 0; i < 100000; i++)
                {
                    txt.WriteLine(rnd.Next(1, 32).ToString());
                   
                }
            }
            
            DateTime d = new DateTime();

            for (int i = 1975; i < 1990; i++)
            {
                Console.WriteLine(DateTime.IsLeapYear(i).ToString(), i.ToString());
                
            }

            Console.WriteLine(DateTime.DaysInMonth(1980, 8));
            Console.WriteLine(DateTime.DaysInMonth(1980, 2));
            Console.WriteLine(DateTime.DaysInMonth(1979, 2));
            */

            string input = "ateeter";

            Console.WriteLine("First unique character in {0} is {1} ", input, GetFirstUniqueCharacter(input));

            Console.ReadKey();
        }
                
        static char GetFirstUniqueCharacter(string x)
        {           
            var input = x.ToList<char>();
            int i = 0;

            var letters = input.Distinct().ToArray();            

            while (!(input.Count(l => l == letters[i]) == 1) 
                   && i < letters.Count())
                i++;                
                                           
            return letters[i];
        }

    }
}
