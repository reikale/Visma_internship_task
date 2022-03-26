using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Visma_internship_task.Interfaces;
using Visma_internship_task.Models;

namespace Visma_internship_task
{
    public class MeetingController
    {
        public IMeeting CreateAMeeting()
        {
            var meetingName = UITools.AnswerQuestion("Set a name for the meeting:");
            var responsiblePerson = UITools.AnswerQuestion("Set a responsible person for the meeting:");
            var description = UITools.AnswerQuestion("Set a description for the meeting:");
            Console.Clear();
            var meetingCategory = (Category)UITools.SelectValue(Enum.GetNames(typeof(Category)), "Select a meeting category:", true);
            Console.Clear();
            var meetingType = (Models.Type)UITools.SelectValue(Enum.GetNames(typeof(Models.Type)), "Select a meeting type:", true);
            var meetingStart = UITools.AnswerDateQuestion("Set a start time for the meeting:");
            var meetingEnd = UITools.AnswerDateQuestion("Set a end time for the meeting:");

            var newMeeting = new Meeting(meetingName, responsiblePerson, description, meetingCategory, meetingType, meetingStart, meetingEnd);
            newMeeting.Attendees.Add(responsiblePerson);
            Console.WriteLine("\n\n");
            Console.WriteLine($"A new meeting named '{newMeeting.Name}' was created");
            Console.WriteLine("\n\n");
            return newMeeting;
        }

        public void DeleteAMeeting(Database database, int selectedMeeting)
        {
            var userInput = UITools.AnswerQuestion("Enter the name of a person who wants to delete this meeting:");
            if(database.AllMeetings[selectedMeeting-1].ResponsiblePerson == userInput)
            {
                Console.WriteLine($"The meeting '{database.AllMeetings[selectedMeeting - 1].Name}' was deleted successfully");
                database.AllMeetings.Remove(database.AllMeetings[selectedMeeting - 1]);
            }
            else
            {
                Console.WriteLine($"{userInput} is not responsible for this meeting therefore does not have the rights to delete it.\nThe meeting was not deleted");
            }
        }

        public void DisplayAllMeetings(IMeeting[] meetings)
        {
            Console.Clear();
            if (meetings.Length == 0)
            {
                var actions = new Action[]
                {
                    () => CreateAMeeting(),
                    () => Console.Clear()
                };
                Console.Clear();
                UITools.SelectValue(new string[] { "YES", "NO" }, "There are no meetings. Would you like to create one?", actions);
            }
            foreach (var meeting in meetings)
            {
                Console.WriteLine(meeting.Name);
            }
            Console.WriteLine("\n\n");
        }

        public string AddPerson(Database database, int selectedMeeting)
        {
            string userInput = UITools.AnswerQuestion("Enter the name of the person you want to add to the meeting:");
            var relevantMeeting = database.AllMeetings[selectedMeeting - 1];
            if (!relevantMeeting.Attendees.Contains(userInput))
            {
                var meetingsPersonAttends = database.AllMeetings.Where(x => x.Attendees.Contains(userInput)).ToList();
                var overlapingMeetings = meetingsPersonAttends.Where(x => x.StartDate<= relevantMeeting.EndDate && x.EndDate >= relevantMeeting.StartDate).ToArray();
                if (overlapingMeetings.Any())
                {
                    Console.WriteLine($"\nWarning! {userInput} has a meeting at this time. The list of intersecting meetings:");
                    for (int i = 0; i < overlapingMeetings.Length; i++)
                    {
                        Console.WriteLine($"{i} - Name: {overlapingMeetings[i].Name} - Start date: {overlapingMeetings[i].StartDate} - End date: {overlapingMeetings[i].EndDate}");
                    }
                }
                relevantMeeting.Attendees.Add(userInput);
                Console.WriteLine($"\n{userInput} was added to the '{relevantMeeting.Name}' meeting at {DateTime.Now}");
            }
            else
            {
                Console.WriteLine($"{userInput} already attends '{relevantMeeting.Name}' meeting");
            }
            return userInput;
        }

        public string RemovePerson(Database database, int selectedMeeting)
        {
            var relevantMeeting = database.AllMeetings[selectedMeeting - 1];
            string userInput = UITools.AnswerQuestion("Enter the name of the person you want to remove from the meeting:");
            if(relevantMeeting.ResponsiblePerson != userInput)
            {
                Console.WriteLine($"{userInput} was removed from the meeting.");
                relevantMeeting.Attendees.Remove(userInput);
            }
            else
            {
                Console.WriteLine($"{userInput} is the responsible person for this meeting therefore cannot be removed");
            }
            return userInput;
        }

        public void FilterMeetingsByDescription(Database database, string question)
        {
            Console.WriteLine(question);
            var userInput = Console.ReadLine();
            if (userInput != null)
            {
                var result = database.AllMeetings.Where(x => x.Description.Contains(userInput)).ToArray();
                Console.Clear();
                if(result.Length == 0)
                {
                    Console.WriteLine($"Sorry, there is no meeting with description that includes '{userInput}'");
                }
                else
                {
                    Console.WriteLine("Filter results:");
                    for (int i = 0; i < result.Length; i++)
                    {
                        Console.WriteLine($"{i} - Name: {result[i].Name} - Description: {result[i].Description}");
                    }
                }
            }
        }

        public void FilterMeetingsByResponsiblePerson(Database database, string responsiblePerson)
        {
            if (responsiblePerson != null)
            {
                var result = database.AllMeetings.Where(x => x.ResponsiblePerson == responsiblePerson).ToArray();
                Console.Clear();
                if (result.Length == 0)
                {
                    Console.WriteLine($"Sorry, there is no meeting where {responsiblePerson} is responsible");
                }
                else
                {
                    Console.WriteLine("Filter results:");
                    for (int i = 0; i < result.Length; i++)
                    {
                        Console.WriteLine($"{i} - Name: {result[i].Name} - Responsible Person: {result[i].ResponsiblePerson}");
                    }
                }
            }
        }

        public void FilterMeetingByCategory(Database database, Category category)
        {
            var result = database.AllMeetings.Where(x => x.Category == category).ToArray();
            Console.Clear();
            if(result.Length == 0)
            {
                Console.WriteLine($"Sorry, there is no meeting of {category} category");
            }
            else
            {
                Console.WriteLine("Filter results:");
                for (int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine($"{i} - Name: {result[i].Name} - Category: {result[i].Category}");
                }
            }
        }
        public void FilterMeetingByType(Database database, Models.Type type)
        {
            var result = database.AllMeetings.Where(x => x.Type == type).ToArray();
            Console.Clear();
            if (result.Length == 0)
            {
                Console.WriteLine($"Sorry there is no meeting of {type} type");
            }
            else
            {
                Console.WriteLine("Filter results:");
                for (int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine($"{i} - Name: {result[i].Name} - Type: {result[i].Type}");
                }
            }
        }

        public void FilterMeetingByDate(Database database)
        {
            var userStartDate = UITools.AnswerDateQuestion("Type a start time for the meeting:");
            var userEndDate = UITools.AnswerDateQuestion("Type a end time for the meeting:");
            
            var filteredMeetings = database.AllMeetings.Where(x => x.StartDate >= userStartDate && x.EndDate <= userEndDate).ToArray();
            if (filteredMeetings.Length == 0)
            {
                Console.WriteLine($"Sorry there is no meeting of selected time period");
            }
            else
            {
                Console.WriteLine("Filter results:");
                for (int i = 0; i < filteredMeetings.Length; i++)
                {
                    Console.WriteLine($"{i} - Name: {filteredMeetings[i].Name} - Start date: {filteredMeetings[i].StartDate} - End date: {filteredMeetings[i].EndDate}");
                }
            }
        }

        public void FilterByAttendees(Database database)
        {
            bool isOn = true;
            int selectedNumber = -1;
            while (isOn)
            {
                string userInput = UITools.AnswerQuestion("Please type the minimum number of attendees to filter the meetings:");
                if (int.TryParse(userInput, out selectedNumber))
                {
                    isOn = false;
                    
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("The input is invalid. Please try again");
                }
            }
            var result = database.AllMeetings.Where(x => x.Attendees.Count >= selectedNumber).ToArray();
            if (result.Length == 0)
            {
                Console.WriteLine($"Sorry there is no meetings with {selectedNumber} attendees");
            }
            else
            {
                Console.WriteLine("Filter results:");
                for (int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine($"{i} - Name: {result[i].Name} - Number of attendees: {result[i].Attendees.Count}");
                }
            }
        }
    }
}
