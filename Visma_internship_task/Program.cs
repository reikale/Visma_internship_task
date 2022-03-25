
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
string[] MeetingOptions = new string[] { "Add a person", "Remove a person", "Delete this meeting", "Go back" };

var startingActions = new Action[]
    {
        () => DB.AddMeetingToDb(meetingController.CreateAMeeting()),
        () => meetingController.DisplayAllMeetings(DB.ReturnAllMeetings()),
        () => {Console.Clear(); isOn=false; }
    };
var meetingActions = new Action[]
{
    () => Console.WriteLine("Adding a person to the meeting"),
    () => Console.WriteLine("Removing a person from the meeting"),
    () => Console.WriteLine("Deleting this meeting"),
    () => Console.WriteLine("Going back"),
};
var meetingListActions = new Action[]
{
    () =>
    {
        Console.Clear();
        selectedMeeting = UITools.SelectValue(DB.AllMeetings.Select(x => x.Name).ToArray(), "Please select a meeting from the list:");
    },
    () => Console.WriteLine("Filter options"),
    () => Console.Clear()
};


// --- THE APP START HERE ---


DB.LoadData();

Console.WriteLine("Welcome to the meetings console app\n\n");

while (isOn)
{
    
    var startOutput = UITools.SelectValue(StartingOptions, "Please select what do you want to do next:", startingActions);
    if(startOutput == 2)
    {
        UITools.SelectValue(MeetingsListOptions, "Please select what do you want to do next:", meetingListActions);
        Console.Clear();
        Console.WriteLine($"You selected the meeting '{DB.AllMeetings[selectedMeeting-1].Name}'\n\n");
        // Meeting options
        
        UITools.SelectValue(MeetingOptions, "Please select what do you want to do next:", meetingActions);
        
        Console.WriteLine("\n");
    }
}


