using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visma_internship_task.Interfaces;
using Visma_internship_task.Models;

namespace Visma_internship_task
{
    public class MeetingController
    {
        // TODO 0: CREATE A MEETING
        public IMeeting CreateAMeeting()
        {

            var meetingName = UITools.AnswerQuestion("Set a name for the meeting:");
            var responsiblePerson = UITools.AnswerQuestion("Set a responsible person for the meeting:");
            var description = UITools.AnswerQuestion("Set a description for the meeting:");
            Console.Clear();
            var meetingCategory = (Category)UITools.SelectValue(Enum.GetNames(typeof(Category)), "Select a meeting category:");
            Console.Clear();
            var meetingType = (Models.Type)UITools.SelectValue(Enum.GetNames(typeof(Models.Type)), "Select a meeting type:");
            var meetingStart = UITools.AnswerDateQuestion("Set a start time for the meeting:");
            var meetingEnd = UITools.AnswerDateQuestion("Set a end time for the meeting:");

            var newMeeting = new Meeting(meetingName, responsiblePerson, description, meetingCategory, meetingType, meetingStart, meetingEnd);
            newMeeting.Attendees.Add(responsiblePerson);
            Console.WriteLine("\n\n");
            Console.WriteLine($"A new meeting named '{newMeeting.Name}' was created");
            Console.WriteLine("\n\n");
            return newMeeting;
        }
        // TODO 1: DELETE A MEETING
        // TODO 2: ADD A PERSON TO THE MEETING
        // TODO 3: REMOVE A PERSON FROM THE MEETING
        // TODO 4: LIST ALL THE MEETINGS
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
            Console.WriteLine("\n\n");
            foreach (var meeting in meetings)
            {
                Console.WriteLine(meeting.Name);
            }
            Console.WriteLine("\n\n");
        }
    }
}
