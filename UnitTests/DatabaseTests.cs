using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visma_internship_task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visma_internship_task.Models;
using System.IO;
using Visma_internship_task.Interfaces;

namespace Visma_internship_task.Tests
{
    [TestClass()]
    public class DatabaseTests
    {
        public Database CreateAMeetingForTests(string name, string person, string description)
        {
            var database = new Database();
            Meeting meeting = CreateOnlyMeeting(name, person, description);
            database.AddMeetingToDb(meeting);
            return database;
        }
        public Meeting CreateOnlyMeeting(string name, string person, string description)
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddMinutes(60);
            var meeting = new Meeting(name, person, description, Category.Hub, Models.Type.InPerson, start, end);
            
            return meeting;
        }

        [DataRow("testName", "TestPerson", "testDescription", "TestPerson")]
        [DataRow("a", "b", "c", "b")]
        [DataTestMethod]
        public void AddMeetingToDbTest(string name, string person, string description, string expectedPerson)
        {
            var database = CreateAMeetingForTests(name, person, description);

            string actualResult = database.AllMeetings.Select(x => x.ResponsiblePerson).SingleOrDefault();

            Assert.IsTrue(database.AllMeetings.Count == 1);
            Assert.AreEqual(actualResult, expectedPerson);
        }

        [DataRow("testName", "TestPerson", "testDescription")]
        [DataRow("a", "b", "c")]
        [DataTestMethod]
        public void LoadDataTest(string name, string person, string description)
        {
            var database = CreateAMeetingForTests(name, person, description);

            var result = database.LoadData();

            Assert.IsNotNull(result);
        }

        [DataRow("testName", "TestPerson", "testDescription", "TestPerson", 1)]
        [DataRow("a", "b", "c", "b", 1)]
        [DataTestMethod]
        public void ReturnAllMeetingsTest(string name, string person, string description, string expectedPerson, int expectedLength)
        {
            var database = CreateAMeetingForTests(name, person, description);

            var output = database.ReturnAllMeetings();

            int actualLength = output.Length;
            string actualPerson = output.Select(x => x.ResponsiblePerson).SingleOrDefault();

            Assert.IsTrue(database.AllMeetings.Count == expectedLength);
            Assert.AreEqual(expectedLength, actualLength);
            Assert.AreEqual(expectedPerson, actualPerson);
        }

        [DataRow("testName", "TestPerson", "testDescription", "TestPerson", 1)]
        [DataRow("a", "b", "c", "b", 1)]
        [DataTestMethod]
        public void ReturnAllResponsiblePeopleTest(string name, string person, string description, string expectedPerson, int expectedLength)
        {
            var database = CreateAMeetingForTests(name, person, description);
            database.AllMeetings.Add(CreateOnlyMeeting(name, person, description));

            var actualLength = database.ReturnAllResponsiblePeople().Length;
            var actualPerson = database.AllMeetings.Select(x => x.ResponsiblePerson).Distinct().ToArray()[0];

            Assert.AreEqual(expectedLength, actualLength);
            Assert.AreEqual(expectedPerson, actualPerson);
        }
    }
}