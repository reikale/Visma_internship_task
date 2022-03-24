
using Visma_internship_task;
using Visma_internship_task.Models;

//ALL THE VARIABLES
bool isOn = true;

var DB = new Database();
var meetingController = new MeetingController();

string[] StartingOptions = new string[] { "Create a meeting", "View all meetings", "Exit the program" };

var startingActions = new Action[]
    {
        () => DB.AddMeetingToDb(meetingController.CreateAMeeting()),
        () => meetingController.DisplayAllMeetings(DB.ReturnAllMeetings()),
        () => {Console.Clear(); isOn=false; }
    };


// --- THE APP START HERE ---


DB.LoadData();

Console.WriteLine("Welcome to the meetings console app");

while (isOn)
{
    
    UITools.SelectValue(StartingOptions, "Please select what do you want to do next:", startingActions);
}


