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

        public List<Meeting> AllMeetings = new List<Meeting>();

        public void AddMeetingToDb(Meeting meeting)
        {
            AllMeetings.Add(meeting);
            SaveData();
        }

        public void SaveData()
        {
            var textData = JsonConvert.SerializeObject(AllMeetings);
            File.WriteAllText(FILE_NAME, textData);
            
        }

        public List<Meeting> LoadData()
        {
            if (File.Exists(FILE_NAME))
            {
                var textData = File.ReadAllText(FILE_NAME);
                
                List<Meeting> MeetingData = JsonConvert.DeserializeObject<List<Meeting>>(textData);
                AllMeetings = MeetingData;
                return AllMeetings;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public Meeting[] ReturnAllMeetings()
        {
            return AllMeetings.ToArray();
        }

        public string[] ReturnAllResponsiblePeople()
        {
            return AllMeetings.Select(x => x.ResponsiblePerson).Distinct().ToArray();
        }
    }
}
