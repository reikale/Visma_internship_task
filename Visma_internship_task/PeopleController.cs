using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visma_internship_task.Interfaces;
using Visma_internship_task.Models;

namespace Visma_internship_task
{
    public class PeopleController
    {
        private MeetingController _meetingController;
        public PeopleController(MeetingController meetingController)
        {
            _meetingController = meetingController;
        }

        public bool CheckForResponsiblePerson(IMeeting relevantMeeting, string userInput)
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
        public string AddPerson(Database database, int selectedMeeting)
        {
            string userInput = UITools.AnswerQuestion("Enter the name of the person you want to add to the meeting:");
            Meeting relevantMeeting = database.AllMeetings[selectedMeeting - 1];
            if (!CheckIfPersonAlreadyInMeeting(relevantMeeting, userInput))
            {
                var meetingsPersonAttends = _meetingController.ReturnListOfMeetingsTheUserAttends(database, userInput);
                var overlapingMeetings = _meetingController.ReturnOverlappingMeetings(meetingsPersonAttends, relevantMeeting);
                _meetingController.ShowAllOverlappingMeetings(overlapingMeetings, userInput);
                AddPersonToDB(relevantMeeting, userInput);
            }
            else
            {
                UITools.UserAlreadyAttendsMeetingMessage(userInput, relevantMeeting);
            }
            return userInput;
        }
        public bool CheckIfPersonAlreadyInMeeting(IMeeting relevantMeeting, string userInput)
        {
            return relevantMeeting.Attendees.Contains(userInput);
        }
        public void AddPersonToDB(IMeeting relevantMeeting, string userInput)
        {
            relevantMeeting.Attendees.Add(userInput);
            UITools.UserWasAddedToMeetingMessage(userInput, relevantMeeting);
        }
        public string RemovePerson(Database database, int selectedMeeting)
        {
            var relevantMeeting = database.AllMeetings[selectedMeeting - 1];
            string userInput = UITools.AnswerQuestion("Enter the name of the person you want to remove from the meeting:");
            if (!CheckForResponsiblePerson(relevantMeeting, userInput))
            {
                UITools.UserWasRemoverFromMeetingMessage(userInput);
                RemovePersonFromDB(relevantMeeting, userInput);
            }
            else
            {
                UITools.UserWasNotRemovedFromMeetingMessage(userInput);
            }
            return userInput;
        }
        public void RemovePersonFromDB(IMeeting relevantMeeting, string userInput)
        {
            relevantMeeting.Attendees.Remove(userInput);
        }
    }

}
