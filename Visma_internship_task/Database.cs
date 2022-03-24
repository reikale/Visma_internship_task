﻿using Newtonsoft.Json;
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

        List<IMeeting> AllMeetings = new List<IMeeting>();
        List<IMeeting> objectData;

        public void AddMeetingToDb(IMeeting meeting)
        {
            AllMeetings.Add(meeting);
            SaveData();
        }
        //should be stored in a JSON file

        //TODO 1: SAVE DATA IN FILE AS JSON
        public void SaveData()
        {
            Console.WriteLine("Data was saved in a file as JSON");
            var textData = JsonConvert.SerializeObject(AllMeetings);
            File.WriteAllText(FILE_NAME, textData);
        }

        //TODO 2: LOAD DATA FROM FILE
        public void LoadData()
        {
            if (File.Exists(FILE_NAME))
            {
                var textData = File.ReadAllText(FILE_NAME);
                
                List<Meeting> MeetingData = JsonConvert.DeserializeObject<List<Meeting>>(textData);
                List<IMeeting> IMeetingData = MeetingData.ToList<IMeeting>();
                AllMeetings = IMeetingData;
                if(objectData != null)
                {
                    AllMeetings = objectData;
                    Console.WriteLine("Data was loaded for a file");
                }
            }
            
        }
        public IMeeting[] ReturnAllMeetings()
        {
            return AllMeetings.ToArray();
        }


    }
}
