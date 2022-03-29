using Visma_internship_task;
using Visma_internship_task.Helpers;
using Visma_internship_task.Models;

// VARIABLES
bool isOn = true;
int selectedMeeting = -1;

// OBJECTS
Database DB = new Database();
MeetingController meetingController = new MeetingController();
PeopleController peopleController = new PeopleController(meetingController);
Filters filters = new Filters(meetingController);
Actions Actions = new Actions(DB, meetingController, filters, peopleController);
meetingController.CreateReferenceToPeopleController(peopleController);

DB.LoadData();

UITools.WelcomeToTheApp();

while (isOn)
{
    var startOutput = UITools.SelectValue(Options.StartingOptions, "Please select what do you want to do next:", Actions.GetStartingActions());
    if (startOutput == 2)
    {
        var selection = UITools.SelectValue(Options.MeetingsListOptions, "Please select what do you want to do next:", Actions.MeetingsListActions());
        if(selectedMeeting > 0)
        {
            UITools.DisplaySelectedMeetingInConsole(DB, selectedMeeting);
        }
        if(selection == 1)
        {
            UITools.SelectValue(Options.MeetingOptions, "Please select what do you want to do next:", Actions.MeetingActions());
        }
        DB.SaveData();
    }
    if (startOutput == 3) isOn = false;
    selectedMeeting = -1;
}


