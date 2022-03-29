using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visma_internship_task.Models;

namespace Visma_internship_task
{
    public class Filters
    {
        private MeetingController _meetingController;
        public Filters(MeetingController meetingController)
        {
            _meetingController = meetingController;
        }

        public void FilterMeetingsByDescription(Database database, string question)
        {
            var userInput = UITools.AnswerQuestion(question);

            if (userInput != null)
            {
                Meeting[] result = _meetingController.ReturnMeetingsWhenDescriptionContains(database, userInput);
                Console.Clear();
                UITools.DisplayFilterResultsByProp(result, userInput, "Description");
            }
        }
        public void FilterMeetingsByResponsiblePerson(Database database, string responsiblePerson)
        {
            if (responsiblePerson != null)
            {
                var result = _meetingController.ReturnMeetingsWhenResponsibleIs(database, responsiblePerson);
                Console.Clear();
                UITools.DisplayFilterResultsByProp(result, "ResponsiblePerson");
            }
        }
        public void FilterMeetingByCategory(Database database, Category category)
        {
            var result = _meetingController.ReturnMeetingsWhenCategory(database, category);
            Console.Clear();
            UITools.DisplayFilterResultsByProp(result, "Category");

        }
        public void FilterMeetingByType(Database database, Models.Type type)
        {
            var result = _meetingController.ReturnMeetingsWhenType(database, type);
            Console.Clear();
            UITools.DisplayFilterResultsByProp(result, "Type");
        }
        public void FilterMeetingByDate(Database database)
        {
            DateTime userStartDate = UITools.AnswerDateQuestion("Type a start time for the meeting:");
            DateTime userEndDate = UITools.AnswerDateQuestion("Type a end time for the meeting:");

            Meeting[] filteredMeetings = _meetingController.ReturnMeetingsByDates(database, userStartDate, userEndDate);
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
            var result = _meetingController.ReturnMeetingsWhenNumberOfAttendeesIs(database, selectedNumber);
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
