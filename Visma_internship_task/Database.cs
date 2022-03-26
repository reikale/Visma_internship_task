using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visma_internship_task.Interfaces;
using Visma_internship_task.Models;

namespace Visma_internship_task
{
    public class Database
    {
        const string FILE_NAME = "Database.json";

        public List<IMeeting> AllMeetings = new List<IMeeting>();

        public void AddMeetingToDb(IMeeting meeting)
        {
            AllMeetings.Add(meeting);
            SaveData();
        }

        public void SaveData()
        {
            var textData = JsonConvert.SerializeObject(AllMeetings);
            File.WriteAllText(FILE_NAME, textData);
        }

        public void LoadData()
        {
            if (File.Exists(FILE_NAME))
            {
                var textData = File.ReadAllText(FILE_NAME);
                
                List<Meeting> MeetingData = JsonConvert.DeserializeObject<List<Meeting>>(textData);
                List<IMeeting> IMeetingData = MeetingData.ToList<IMeeting>();
                AllMeetings = IMeetingData;
            }
        }

        public IMeeting[] ReturnAllMeetings()
        {
            return AllMeetings.ToArray();
        }

        public string[] ReturnAllResponsiblePeople()
        {
            return AllMeetings.Select(x => x.ResponsiblePerson).Distinct().ToArray();
        }
    }
}
