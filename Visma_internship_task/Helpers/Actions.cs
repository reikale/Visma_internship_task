using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visma_internship_task;
using Visma_internship_task.Models;

namespace Visma_internship_task.Helpers
{

    public class Actions
    {
        public Database _database;
        private readonly MeetingController _meetingController;
        private readonly Filters _filters;
        private readonly PeopleController _peopleController;
        private int _selectedMeeting = -1;

        public Actions(Database database, MeetingController meetingController, Filters filters, PeopleController peopleController)
        {
            _database = database;
            _meetingController = meetingController;
            _filters = filters;
            _peopleController = peopleController;
        }

        public Action[] GetStartingActions() => new Action[]
        {
            () => _database.AddMeetingToDb(_meetingController.CreateAMeeting()),
            () => DisplayAllMeetings(),
            () => Console.Clear()
        };
        public Action[] MeetingActions() => new Action[]
        {
            () => AddPersonToAttendees(),
            () => RemovePersonFromAttendees(),
            () => _meetingController.DeleteAMeeting(_database, _selectedMeeting),
            () => Console.Clear()
        };

        public Action[] FilterActions() => new Action[]
        {
            () => _filters.FilterMeetingsByDescription(_database, "Enter the keyword by which to you want to filter the meetings:"),
            () => SelectFilterByResponsiblePerson(),
            () => SelectFilterByCategory(),
            () => SelectFilterByType(),
            () => _filters.FilterMeetingByDate(_database),
            () => _filters.FilterByAttendees(_database)

            };
        public Action[] MeetingsListActions() => new Action[]
        {
            () => SelectAMeeting(),
            () => SelectAFilter(),
            () => Console.Clear()
        };

        public void DisplayAllMeetings()
        {
            int nextMove = _meetingController.DisplayAllMeetings(_database.ReturnAllMeetings());
            if(nextMove < 0)
            {
                _selectedMeeting = -1;

            }
        }
        public void AddPersonToAttendees()
        {
            _peopleController.AddPerson(_database, _selectedMeeting);
            Console.WriteLine("\nList of attendees:");
            foreach (var attendee in _database.AllMeetings[_selectedMeeting - 1].Attendees)
            {
                Console.WriteLine(attendee);
            }
        }

        public void RemovePersonFromAttendees()
        {
            _peopleController.RemovePerson(_database, _selectedMeeting);
            Console.WriteLine("\nList of attendees:");
            foreach (var attendee in _database.AllMeetings[_selectedMeeting - 1].Attendees)
            {
                Console.WriteLine(attendee);
            }
        }

        public void SelectFilterByResponsiblePerson()
        {
            int selection = UITools.SelectValue(_database.ReturnAllResponsiblePeople(), "Select a responsible person from the list:", false);
            var selectedPerson = _database.ReturnAllResponsiblePeople()[selection - 1];
            _filters.FilterMeetingsByResponsiblePerson(_database, selectedPerson);
        }

        public void SelectFilterByCategory()
        {
            Console.Clear();
            var meetingCategory = (Category)UITools.SelectValue(Enum.GetNames(typeof(Category)), "Select a meeting category from the list:", true);
            _filters.FilterMeetingByCategory(_database, meetingCategory);
        }

        public void SelectFilterByType()
        {
            var meetingType = (Visma_internship_task.Models.Type)UITools.SelectValue(Enum.GetNames(typeof(Visma_internship_task.Models.Type)), "Select a meeting type:", true);
            _filters.FilterMeetingByType(_database, meetingType);
        }

        public void SelectAMeeting()
        {
            Console.Clear();

            _selectedMeeting = UITools.SelectValue(_database.AllMeetings.Select(x => x.Name).ToArray(), "Please select a meeting from the list:", false);
        }

        public void SelectAFilter()
        {
            Console.Clear();
            UITools.SelectValue(Options.FilterOptions, "Please select a filter.\nFilter by...\n", FilterActions());
        }
    }
}
