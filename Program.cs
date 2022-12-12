using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tally_Assignment2
{
    internal class Program
    {


        static void Main(string[] args)
        {
            bool exit = true;
            while (exit)
            {
                Console.WriteLine("1 -- Add     2--Update   3--DisplayByID  4--DisplayAll  5--Delete  6--Exit");
                StudentOperation operations = new StudentOperation();
                Console.WriteLine("Enter the option");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        operations.AddStudent();
                        break;

                    case 2:
                        Console.WriteLine("Enter the student id to update:");
                        int sid = Convert.ToInt32(Console.ReadLine());
                        operations.UpdateStudent(sid);
                        break;

                    case 3:
                        Console.WriteLine("Enter the student id to Display");
                        int sid1 = Convert.ToInt32(Console.ReadLine());
                        operations.DisplayById(sid1);
                        break;

                    case 4:
                        
                        operations.DisplayAll();
                        break;

                    case 5:
                        Console.WriteLine("Enter the student id to Delete");
                        int sid3 = Convert.ToInt32(Console.ReadLine());
                        operations.DeleteStudent(sid3);
                        break;

                    case 6:
                        exit = false;
                        break;

                }
            }
            Console.ReadKey();
        }
    }
}
