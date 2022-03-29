using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Visma_internship_task.ConsoleNameRetriever;

namespace Visma_internship_task
{
    public class UITools : ConsoleNameRetriever
    {

        public override string GetNextName()
        {
            return base.GetNextName();
        }
        public static int SelectValue(string[] options, string question, Action[] actions)
        {
            bool exit = false;
            int selection = 0;

            while (!exit)
            {
                Console.WriteLine(question);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"{i + 1} - {options[i]}");
                }
                
                if (!int.TryParse(Console.ReadLine(), out selection) || selection > options.Length || selection < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Incorrect value. Please make sure that the input is a number");
                    continue;
                }
                exit = true;
                actions[selection-1]();
            }
            return selection;
        }
        
        public static int SelectValue(string[] options, string question, bool isEnum)
        {
            bool exit = false;
            int selection = 0;

            while (!exit)
            {
                Console.WriteLine(question);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"{i+1} - {options[i]}");
                }

                if (!int.TryParse(Console.ReadLine(), out selection) || selection > options.Length || selection < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Incorrect value. Please make sure that the input is a number");
                    continue;
                }
                exit = true;
            }
            if (isEnum)
            {
                selection--;
            }
            return selection;
        }
        
        public static string AnswerQuestion(string question)
        {
            bool exit = false;
            var userInput = "none";
            Console.Clear();
            while (!exit)
            {
                Console.WriteLine(question);
                userInput = Console.ReadLine();
                if (userInput == null)
                {
                    Console.WriteLine("The input is invalid. Please try again");
                    continue;
                }
                exit = true;
            }
            return userInput;
        }

        public static DateTime AnswerDateQuestion(string question)
        {
            Console.Clear();
            while (true)
            {
                    Console.WriteLine(question);
                    var userInput = Console.ReadLine();
                    DateTime newDate = new DateTime();
                    if (DateTime.TryParse(userInput, out newDate))
                    {
                        return newDate;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("The input is invalid. Please try again");
                    }
            }
        }
    }
}
