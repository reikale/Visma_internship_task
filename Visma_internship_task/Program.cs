
using Visma_internship_task;
using Visma_internship_task.Interfaces;
using Visma_internship_task.Models;

//ALL THE VARIABLES
bool isOn = true;
int selectedMeeting = -1;

var DB = new Database();
var meetingController = new MeetingController();

string[] StartingOptions = new string[] { "Create a meeting", "View all meetings", "Exit the program" };
string[] MeetingsListOptions = new string[] { "Select a meeting", "Filter all meetings by...", "Go back" };
string[] MeetingOptions = new string[] { "Add a person to this meeting", "Remove a person from this meeting", "Delete this meeting", "Go back" };
string[] FilterOptions = new string[] { "Description", "Responsible person", "Category", "Type", "Dates", "Number of attendees" };

var startingActions = new Action[]
    {
        () => DB.AddMeetingToDb(meetingController.CreateAMeeting()),
        () => meetingController.DisplayAllMeetings(DB.ReturnAllMeetings()),
        () => {Console.Clear(); isOn=false; }
    };
var meetingActions = new Action[]
{
    () => {
        meetingController.AddPerson(DB, selectedMeeting);
        global::System.Console.WriteLine("\nList of attendees:");
        foreach (var attendee in DB.AllMeetings[selectedMeeting-1].Attendees)
    {
            global::System.Console.WriteLine(attendee);
    }
    },
    () => {
        meetingController.RemovePerson(DB, selectedMeeting);
        global::System.Console.WriteLine("\nList of attendees:");
        foreach (var attendee in DB.AllMeetings[selectedMeeting-1].Attendees)
    {
            global::System.Console.WriteLine(attendee);
    }
    },
    () =>
    {
        
        meetingController.DeleteAMeeting(DB, selectedMeeting);
        
    },
    () => Console.Clear(),
};
var filterActions = new Action[]
{
    () => meetingController.FilterMeetingsByDescription(DB, "Enter the keyword by which to filter:"),
    () => {
        int selection = UITools.SelectValue(DB.ReturnAllResponsiblePeople(), "Select a responsible person from the list:", false);
        var selectedPerson = DB.ReturnAllResponsiblePeople()[selection-1];
        meetingController.FilterMeetingsByResponsiblePerson(DB, selectedPerson);
    },
    () => {
        Console.Clear();
        var meetingCategory = (Category)UITools.SelectValue(Enum.GetNames(typeof(Category)), "Select a meeting category from the list:", true);
        meetingController.FilterMeetingByCategory(DB, meetingCategory);
    },
    () => {
        var meetingType = (Visma_internship_task.Models.Type)UITools.SelectValue(Enum.GetNames(typeof(Visma_internship_task.Models.Type)), "Select a meeting type:", true);
        meetingController.FilterMeetingByType(DB, meetingType);
    },
    () => meetingController.FilterMeetingByDate(DB),
    () => meetingController.FilterByAttendees(DB)
};
var meetingListActions = new Action[]
{
    () =>
    {
        Console.Clear();
        selectedMeeting = UITools.SelectValue(DB.AllMeetings.Select(x => x.Name).ToArray(), "Please select a meeting from the list:", false);
    },
    () => {
        Console.Clear();
        UITools.SelectValue(FilterOptions, "Please select a filter.\nFilter by...\n", filterActions); 

    },
    () => Console.Clear()
};


// --- THE APP START HERE ---


DB.LoadData();

Console.WriteLine("Welcome to the meetings console app\n\n");

while (isOn)
{
    
    var startOutput = UITools.SelectValue(StartingOptions, "Please select what do you want to do next:", startingActions);
    
    if (startOutput == 2)
    {
        
        var selection = UITools.SelectValue(MeetingsListOptions, "Please select what do you want to do next:", meetingListActions);
        //Console.Clear();
        if(selectedMeeting > 0)
        {
            Console.WriteLine($"You selected the meeting '{DB.AllMeetings[selectedMeeting - 1].Name}'\n\n");
        }
        switch (selection)
        {
            // jei nebus daugiau casu tai padaryt tiesiog if statementa
            case 1:
                UITools.SelectValue(MeetingOptions, "Please select what do you want to do next:", meetingActions);
                break;
            default:
                break;
        }
        // Meeting options

        
        
        Console.WriteLine("\n");
        DB.SaveData();
    }
    selectedMeeting = -1;
}


