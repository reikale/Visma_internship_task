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
        public Meeting CreateAMeeting()
        {
            string meetingName = string.Empty;
            string responsiblePerson = string.Empty;
            string description = string.Empty;
            Category meetingCategory = Category.TeamBuilding;
            Models.Type meetingType = Models.Type.Live;
            DateTime meetingStart = DateTime.Now;
            DateTime meetingEnd = DateTime.Now;

            AskQuestionsForMeetingCreation(
            ref meetingName,
            ref responsiblePerson,
            ref description,
            ref meetingCategory,
            ref meetingType,
            ref meetingStart,
            ref meetingEnd);

            var newMeeting = CreateMeetingObject(meetingName, responsiblePerson, description, meetingCategory, meetingType, meetingStart, meetingEnd);
            return newMeeting;
        }
        
        public void AskQuestionsForMeetingCreation(
            ref string meetingName, 
            ref string responsiblePerson, 
            ref string description, 
            ref Category meetingCategory, 
            ref Models.Type meetingType, 
            ref DateTime meetingStart, 
            ref DateTime meetingEnd)
        {
            meetingName = UITools.AnswerQuestion("Set a name for the meeting:");
            responsiblePerson = UITools.AnswerQuestion("Set a responsible person for the meeting:");
            description = UITools.AnswerQuestion("Set a description for the meeting:");
            Console.Clear();
            meetingCategory = (Category)UITools.SelectValue(Enum.GetNames(typeof(Category)), "Select a meeting category:", true);
            Console.Clear();
            meetingType = (Models.Type)UITools.SelectValue(Enum.GetNames(typeof(Models.Type)), "Select a meeting type:", true);
            bool isOn = true;
            while (isOn)
            {
                meetingStart = UITools.AnswerDateQuestion("Set a start time for the meeting:");
                meetingEnd = UITools.AnswerDateQuestion("Set a end time for the meeting:");
                if(meetingStart > meetingEnd)
                {
                    Console.WriteLine("The time of the start can't be later than the end of the meeting. Please try again.");
                    continue;
                }
                isOn = false;
            }
            
        }

        public Meeting CreateMeetingObject(string meetingName, string responsiblePerson, string description, Category meetingCategory, Models.Type meetingType, DateTime meetingStart, DateTime meetingEnd)
        {
            if(string.IsNullOrWhiteSpace(meetingName) || string.IsNullOrWhiteSpace(responsiblePerson) || string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException();
            }
            var output = new Meeting(meetingName, responsiblePerson, description, meetingCategory, meetingType, meetingStart, meetingEnd);
            output.Attendees.Add(responsiblePerson);
            Console.WriteLine($"\n\nA new meeting named '{output.Name}' was created\n\n");
            return output;
        }

        public void DeleteAMeeting(Database database, int selectedMeeting)
        {
            var relevantMeeting = database.AllMeetings[selectedMeeting - 1];
            var userInput = UITools.AnswerQuestion("Enter the name of a person who wants to delete this meeting:");
            bool isTheRightPerson = CheckForResponsiblePerson(relevantMeeting, userInput);
            if (isTheRightPerson)
            {
                RemoveMeetingFromDB(relevantMeeting, database);
            }
            else
            {
                Console.WriteLine($"{userInput} is not responsible for this meeting therefore does not have the rights to delete it.\nThe meeting was not deleted");
            }
        }
        
        public bool CheckForResponsiblePerson(Meeting relevantMeeting, string userInput)
        {
            if (relevantMeeting.ResponsiblePerson == userInput)
            {
                return true;
            }
            else
            {
               return false;
            }
        }
        
        public Database RemoveMeetingFromDB(Meeting relevantMeeting, Database database)
        {
            Console.WriteLine($"The meeting '{relevantMeeting.Name}' was deleted successfully");
            database.AllMeetings.Remove(relevantMeeting);
            return database;
        }

        public void DisplayAllMeetings(Meeting[] meetings)
        {
            Console.Clear();
            CreateAMeetingIfThereAreNone(meetings);
            foreach (var meeting in meetings)
            {
                Console.WriteLine(meeting.Name);
            }
            Console.WriteLine("\n\n");
        }

        public string CreateAMeetingIfThereAreNone(Meeting[] meetings)
        {
            if (meetings.Length == 0)
            {
                var actions = new Action[]
                {
                    () => CreateAMeeting(),
                    () => Console.Clear()
                };
                
                UITools.SelectValue(new string[] { "YES", "NO" }, "\nThere are no meetings. Would you like to create one?", actions);
                return "There are no meetings, therefore user was suggested to create one";
            }
            return $"There are {meetings.Length} meetings";
        }

        public string AddPerson(Database database, int selectedMeeting)
        {
            string userInput = UITools.AnswerQuestion("Enter the name of the person you want to add to the meeting:");
            Meeting relevantMeeting = database.AllMeetings[selectedMeeting - 1];
            if (CheckIfPersonAlreadyInMeeting(relevantMeeting, userInput))
            {
                var meetingsPersonAttends = ReturnListOfMeetingsTheUserAttends(database, userInput);
                var overlapingMeetings = ReturnOverlappingMeetings(meetingsPersonAttends, relevantMeeting);
                ShowAllOverlappingMeetings(overlapingMeetings, userInput);
                AddPersonToDB(relevantMeeting, userInput);
            }
            else
            {
                Console.WriteLine($"{userInput} already attends '{relevantMeeting.Name}' meeting");
            }
            return userInput;
        }
        
        public bool CheckIfPersonAlreadyInMeeting(Meeting relevantMeeting, string userInput)
        {
            if (!relevantMeeting.Attendees.Contains(userInput))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public void AddPersonToDB(Meeting relevantMeeting, string userInput)
        {
            relevantMeeting.Attendees.Add(userInput);
            Console.WriteLine($"\n{userInput} was added to the '{relevantMeeting.Name}' meeting at {DateTime.Now}");
        }
        public List<Meeting> ReturnListOfMeetingsTheUserAttends(Database database, string userInput)
        {
            List<Meeting> meetingsPersonAttends = database.AllMeetings.Where(x => x.Attendees.Contains(userInput)).ToList();
            return meetingsPersonAttends;
        }

        public Meeting[] ReturnOverlappingMeetings(List<Meeting> meetingsPersonAttends, Meeting relevantMeeting)
        {
            Meeting[] fullyOverlapingMeetings = meetingsPersonAttends.Where(x => x.StartDate <= relevantMeeting.EndDate && x.EndDate >= relevantMeeting.StartDate).ToArray();
            Meeting[] StartOverlapingMeetings = meetingsPersonAttends.Where(x => x.StartDate >= relevantMeeting.StartDate && x.StartDate <= relevantMeeting.EndDate).ToArray();
            Meeting[] EndOverlapingMeetings = meetingsPersonAttends.Where(x => x.EndDate >= relevantMeeting.StartDate && x.EndDate <= relevantMeeting.EndDate).ToArray();
            Meeting[] output = fullyOverlapingMeetings.Union(StartOverlapingMeetings).Union(EndOverlapingMeetings).ToArray();
            return output;
        }
        public void ShowAllOverlappingMeetings(Meeting[] overlapingMeetings, string userInput)
        {
            if (overlapingMeetings.Any())
            {
                Console.WriteLine($"\nWarning! {userInput} has a meeting at this time. The list of intersecting meetings:");
                for (int i = 0; i < overlapingMeetings.Length; i++)
                {
                    Console.WriteLine($"{i} - Name: {overlapingMeetings[i].Name} - Start date: {overlapingMeetings[i].StartDate} - End date: {overlapingMeetings[i].EndDate}");
                }
            }
        }
        public string RemovePerson(Database database, int selectedMeeting)
        {
            var relevantMeeting = database.AllMeetings[selectedMeeting - 1];
            string userInput = UITools.AnswerQuestion("Enter the name of the person you want to remove from the meeting:");
            if(!CheckForResponsiblePerson(relevantMeeting, userInput))
            {
                Console.WriteLine($"{userInput} was removed from the meeting.");
                RemovePersonFromDB(relevantMeeting, userInput);
            }
            else
            {
                Console.WriteLine($"{userInput} is the responsible person for this meeting therefore cannot be removed");
            }
            return userInput;
        }

        public void RemovePersonFromDB(Meeting relevantMeeting, string userInput)
        {
            relevantMeeting.Attendees.Remove(userInput);
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
