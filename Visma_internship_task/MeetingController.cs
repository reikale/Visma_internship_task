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
        private PeopleController _peopleController;
        public MeetingController()
        {
            
        }
        public void CreateReferenceToPeopleController(PeopleController peopleController)
        {
            _peopleController = peopleController;
        }
        public Meeting CreateAMeeting()
        {
            string meetingName = string.Empty;
            string responsiblePerson = string.Empty;
            string description = string.Empty;
            Category meetingCategory = Category.TeamBuilding;
            Models.Type meetingType = Models.Type.Live;
            DateTime meetingStart = DateTime.Now;
            DateTime meetingEnd = DateTime.Now;

            UITools.AskQuestionsForMeetingCreation(
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


        /// <summary>
        /// Cia aprašymas
        /// </summary>
        /// <param name="meetingName">Šitas parametras nurodo susirinkimo vardą</param>
        /// <param name="responsiblePerson"></param>
        /// <param name="description"></param>
        /// <param name="meetingCategory"></param>
        /// <param name="meetingType"></param>
        /// <param name="meetingStart"></param>
        /// <param name="meetingEnd"></param>
        /// <returns>Grąžina meeting objektą</returns>
        /// <exception cref="ArgumentException">Jei kažkas negerai - exception</exception>

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
            Meeting relevantMeeting = database.AllMeetings[selectedMeeting - 1];
            var userInput = UITools.AnswerQuestion("Enter the name of a person who wants to delete this meeting:");
            bool isTheRightPerson = _peopleController.CheckForResponsiblePerson(relevantMeeting, userInput);
            if (isTheRightPerson)
            {
                RemoveMeetingFromDB(relevantMeeting, database);
            }
            else
            {
                UITools.UserIsNotResponsibleAndCantDeleteMeetingMessage(userInput);
            }
        }

        public Database RemoveMeetingFromDB(Meeting relevantMeeting, Database database)
        {
            UITools.MeetingDeletedSuccessfullyMessage(relevantMeeting);
            database.AllMeetings.Remove(relevantMeeting);
            return database;
        }

        public void DisplayAllMeetings(IMeeting[] meetings)
        {
            Console.Clear();
            CreateAMeetingIfThereAreNone(meetings);
            foreach (var meeting in meetings)
            {
                Console.WriteLine(meeting.Name);
            }
            Console.WriteLine("\n\n");
        }

        public string CreateAMeetingIfThereAreNone(IMeeting[] meetings)
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

        public List<Meeting> ReturnListOfMeetingsTheUserAttends(Database database, string userInput)
        {
            List<Meeting> meetingsPersonAttends = database.AllMeetings.Where(x => x.Attendees.Contains(userInput)).ToList();
            return meetingsPersonAttends;
        }

        public Meeting[] ReturnOverlappingMeetings(List<Meeting> meetingsPersonAttends, IMeeting relevantMeeting)
        {
            Meeting[] fullyOverlapingMeetings = meetingsPersonAttends.Where(x => x.StartDate <= relevantMeeting.EndDate && x.EndDate >= relevantMeeting.StartDate).ToArray();
            Meeting[] StartOverlapingMeetings = meetingsPersonAttends.Where(x => x.StartDate >= relevantMeeting.StartDate && x.StartDate <= relevantMeeting.EndDate).ToArray();
            Meeting[] EndOverlapingMeetings = meetingsPersonAttends.Where(x => x.EndDate >= relevantMeeting.StartDate && x.EndDate <= relevantMeeting.EndDate).ToArray();
            Meeting[] output = fullyOverlapingMeetings.Union(StartOverlapingMeetings).Union(EndOverlapingMeetings).ToArray();
            return output;
        }
        public void ShowAllOverlappingMeetings(IMeeting[] overlapingMeetings, string userInput)
        {
            if (overlapingMeetings.Any())
            {
                UITools.WarningMessageAboutOverlapping(userInput);
                for (int i = 0; i < overlapingMeetings.Length; i++)
                {
                    UITools.OverlapingMeetingItem(overlapingMeetings, i);
                }
            }
        }
        
        public Meeting[] ReturnMeetingsWhenDescriptionContains(Database database, string userInput)
        {
            Meeting[] output = database.AllMeetings.Where(x => x.Description.Contains(userInput)).ToArray();
            return output;
        }

        public Meeting[] ReturnMeetingsWhenResponsibleIs(Database database, string responsiblePerson)
        {
            Meeting[] output = database.AllMeetings.Where(x => x.ResponsiblePerson == responsiblePerson).ToArray();
            return output;
        }
        
        public Meeting[] ReturnMeetingsWhenCategory(Database database, Category category)
        {
            Meeting[] output = database.AllMeetings.Where(x => x.Category == category).ToArray();
            return output;
        }

        public Meeting[] ReturnMeetingsWhenType(Database database, Models.Type type)
        {
            Meeting[] output = database.AllMeetings.Where(x => x.Type == type).ToArray();
            return output;
        }

        public Meeting[] ReturnMeetingsByDates(Database database, DateTime userStartDate, DateTime userEndDate)
        {
            Meeting[] output = database.AllMeetings.Where(x => x.StartDate >= userStartDate && x.EndDate <= userEndDate).ToArray(); database.AllMeetings.Where(x => x.StartDate >= userStartDate && x.EndDate <= userEndDate).ToArray();
            return output;
        }

        public Meeting[] ReturnMeetingsWhenNumberOfAttendeesIs(Database database, int selectedNumber)
        {
            Meeting[] output = database.AllMeetings.Where(x => x.Attendees.Count >= selectedNumber).ToArray();
            return output;
        }
    }
}
