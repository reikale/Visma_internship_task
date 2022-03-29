using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visma_internship_task.Helpers
{
    public static class Options
    {
        public static string[] StartingOptions = new string[] { "Create a meeting", "View all meetings", "Exit the program" };
        public static string[] MeetingsListOptions = new string[] { "Select a meeting", "Filter all meetings by...", "Go back" };
        public static string[] MeetingOptions = new string[] { "Add a person to this meeting", "Remove a person from this meeting", "Delete this meeting", "Go back" };
        public static string[] FilterOptions = new string[] { "Description", "Responsible person", "Category", "Type", "Dates", "Number of attendees" };
    }
}
