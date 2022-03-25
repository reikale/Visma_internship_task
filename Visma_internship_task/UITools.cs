using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visma_internship_task
{
    public static class UITools
    {
        

        public static int SelectValue(string[] options, string question, Action[] actions)
        {
            //Console.Clear();
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
        
        public static int SelectValue(string[] options, string question)
        {
            //Console.Clear();
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
            return selection;
        }
        /*public static int SelectMeeting(string[] options, string question)
        {
            //Console.Clear();
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
                Console.WriteLine($"Selected a meeting named {}");
            }
            return selection;
        }*/
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
            bool exit = false;

            Console.Clear();
            while (!exit)
            {
                while (true)
                {
                    Console.WriteLine(question);
                    var userInput = Console.ReadLine();
                    DateTime newDate = new DateTime();
                    if (DateTime.TryParse(userInput, out newDate))
                    {
                        exit = true;
                        return newDate;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("The input is invalid. Please try again");
                    }
                }
            }
            return DateTime.Now;
        }

    }
}
