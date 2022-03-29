using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Visma_internship_task.Interfaces;
using Visma_internship_task.Models;

namespace Visma_internship_task
{
    public class UITools
    {
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
        public static void AskQuestionsForMeetingCreation(
            ref string meetingName,
            ref string responsiblePerson,
            ref string description,
            ref Category meetingCategory,
            ref Models.Type meetingType,
            ref DateTime meetingStart,
            ref DateTime meetingEnd)
        {
            meetingName = AnswerQuestion("Set a name for the meeting:");
            responsiblePerson = AnswerQuestion("Set a responsible person for the meeting:");
            description = AnswerQuestion("Set a description for the meeting:");
            Console.Clear();
            meetingCategory = (Category)SelectValue(Enum.GetNames(typeof(Category)), "Select a meeting category:", true);
            Console.Clear();
            meetingType = (Models.Type)SelectValue(Enum.GetNames(typeof(Models.Type)), "Select a meeting type:", true);
            bool isOn = true;
            while (isOn)
            {
                meetingStart = AnswerDateQuestion("Set a start time for the meeting:");
                meetingEnd = AnswerDateQuestion("Set a end time for the meeting:");
                if (meetingStart > meetingEnd)
                {
                    Console.WriteLine("The time of the start can't be later than the end of the meeting. Please try again.");
                    continue;
                }
                isOn = false;
            }

        }

        public static string DisplayFilterResultsByProp(IMeeting[] result, string userInput, string propName)
        {
            if (result.Length == 0)
            {
                Console.WriteLine($"Sorry, there is no such {propName}");
                return $"Sorry, there is no meeting with {propName} that includes '{userInput}'";
            }
            else
            {
                Console.WriteLine("Filter results:");
                for (int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine($"{i} - Name: {result[i].Name} - {propName}: {result[i].GetType().GetProperty(propName).GetValue(result[i], null)}");
                }
                Console.WriteLine();
                return $"Showing results of {result.Length} meetings";
            }
        }
        public static string DisplayFilterResultsByProp(IMeeting[] result, string propName)
        {
            if (result.Length == 0)
            {
                Console.WriteLine($"Sorry, there is no such {propName.ToLowerInvariant}");
                return $"Sorry, there is no meeting with that {propName.ToLowerInvariant}";
            }
            else
            {
                Console.WriteLine("Filter results:");
                for (int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine($"{i} - Name: {result[i].Name} - {propName}: {result[i].GetType().GetProperty(propName).GetValue(result[i], null)}");
                }
                Console.WriteLine();
                return $"Showing results of {result.Length} meetings";
            }
            
        }

        public static void WelcomeToTheApp()
        {
            Console.WriteLine("Welcome to the meetings console app\n\n");
        }
        public static void DisplaySelectedMeetingInConsole(Database DB, int selectedMeeting)
        {
            Console.WriteLine($"You selected the meeting '{DB.AllMeetings[selectedMeeting - 1].Name}'\n\n");
        }
        public static void UserAlreadyAttendsMeetingMessage(string userInput, Meeting relevantMeeting)
        {
            Console.WriteLine($"{userInput} already attends '{relevantMeeting.Name}' meeting");
        }
        public static void UserWasAddedToMeetingMessage(string userInput, IMeeting relevantMeeting)
        {
            Console.WriteLine($"\n{userInput} was added to the '{relevantMeeting.Name}' meeting at {DateTime.Now}");
        }
        public static void UserWasRemoverFromMeetingMessage(string userInput)
        {
            Console.WriteLine($"{userInput} was removed from the meeting.");
        }
        public static void UserWasNotRemovedFromMeetingMessage(string userInput)
        {
            Console.WriteLine($"{userInput} is the responsible person for this meeting therefore cannot be removed");
        }
        public static void UserIsNotResponsibleAndCantDeleteMeetingMessage(string userInput)
        {
            Console.WriteLine($"{userInput} is not responsible for this meeting therefore does not have the rights to delete it.\nThe meeting was not deleted");
        }
        public static void MeetingDeletedSuccessfullyMessage(IMeeting relevantMeeting)
        {
            Console.WriteLine($"The meeting '{relevantMeeting.Name}' was deleted successfully");
        }
        public static void WarningMessageAboutOverlapping(string userInput)
        {
            Console.WriteLine($"\nWarning! {userInput} has a meeting at this time. The list of intersecting meetings:");
        }
        public static void OverlapingMeetingItem(IMeeting[] overlapingMeetings, int i)
        {
            Console.WriteLine($"{i} - Name: {overlapingMeetings[i].Name} - Start date: {overlapingMeetings[i].StartDate} - End date: {overlapingMeetings[i].EndDate}");
        }
    }
}
