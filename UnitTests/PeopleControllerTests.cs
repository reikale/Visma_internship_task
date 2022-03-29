using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visma_internship_task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visma_internship_task.Models;

namespace Visma_internship_task.Tests
{
    [TestClass()]
    public class PeopleControllerTests
    {
        private Database _database;
        private MeetingController _meetingsController;
        private PeopleController _peopleController;
        private Filters _filter;

        [TestInitialize]
        public void Setup()
        {
            _database = new Database();
            _meetingsController = new MeetingController();
            _peopleController = new PeopleController(_meetingsController);
            _meetingsController.CreateReferenceToPeopleController(_peopleController, _database);
            _filter = new Filters(_meetingsController);
        }
        [DataRow("testName", "testResponsiblePerson", "testDescription", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01", "testResponsiblePerson")]
        [DataRow("a", "b", "c", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01", "b")]
        [DataTestMethod]
        public void CheckForResponsiblePersonTest_ShouldWork(string meetingName, string responsiblePerson, string description, Category meetingCategory, Models.Type meetingType, string meetingStart, string meetingEnd, string expectedPerson)
        {

            DateTime startDateInput = DateTime.Parse(meetingStart);
            DateTime endDateInput = DateTime.Parse(meetingEnd);
            Meeting meeting = new Meeting(meetingName, responsiblePerson, description, meetingCategory, meetingType, startDateInput, endDateInput);

            bool actualResult = _peopleController.CheckForResponsiblePerson(meeting, expectedPerson);

            Assert.IsTrue(actualResult);
        }

        [DataRow("testName", "testResponsiblePerson", "testDescription", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01", "b")]
        [DataRow("a", "b", "c", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01", "testResponsiblePerson")]
        [DataTestMethod]
        public void CheckForResponsiblePersonTest_Shouldfail(string meetingName, string responsiblePerson, string description, Category meetingCategory, Models.Type meetingType, string meetingStart, string meetingEnd, string expectedPerson)
        {
            DateTime startDateInput = DateTime.Parse(meetingStart);
            DateTime endDateInput = DateTime.Parse(meetingEnd);
            Meeting meeting = new Meeting(meetingName, responsiblePerson, description, meetingCategory, meetingType, startDateInput, endDateInput);

            bool actualResult = _peopleController.CheckForResponsiblePerson(meeting, expectedPerson);

            Assert.IsFalse(actualResult);
        }

        [DataRow("NameTrue", "NameTrue", true)]
        [DataRow("NameTrue", "NameFalse", false)]
        [DataRow("0", "0", true)]
        [DataTestMethod]
        public void CheckIfPersonAlreadyInMeetingTest(string name, string userInput, bool expected)
        {
            //Arrange
            Meeting meeting = _meetingsController.CreateMeetingObject("Test", name, "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
            _database.AddMeetingToDb(meeting);
            Meeting relevantMeeting = _database.AllMeetings.Where(x => x.Name == "Test").SingleOrDefault();
            //Act
            bool actual = _peopleController.CheckIfPersonAlreadyInMeeting(relevantMeeting, userInput);

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [DataRow("NewUser", 2)]
        [DataTestMethod()]
        public void AddPersonToDBTest(string name, int expected)
        {
            // Arrange
            Meeting meeting = _meetingsController.CreateMeetingObject("Test", "TestUser", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));

            _database.AddMeetingToDb(meeting);
            Meeting relevantMeeting = _database.AllMeetings.Where(x => x.Name == "Test").FirstOrDefault();


            _peopleController.AddPersonToDB(relevantMeeting, name);
            int actual = _database.AllMeetings.Where(x => x.Name == "Test").FirstOrDefault().Attendees.Count;

            Assert.AreEqual(expected, actual);
        }
        [DataRow(1)]
        [DataTestMethod()]
        public void RemovePersonFromDBTest(int expected)
        {
            // Arrange
            Meeting meeting = _meetingsController.CreateMeetingObject("Test", "TestUser", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));

            _database.AddMeetingToDb(meeting);
            Meeting relevantMeeting = _database.AllMeetings.Where(m => m.Name == "Test").SingleOrDefault();
            relevantMeeting.Attendees.Add("UserToRemove");
            _peopleController.RemovePersonFromDB(relevantMeeting, "UserToRemove");

            // Act
            int actual = relevantMeeting.Attendees.Count;
            bool isThereThisUser = _database.AllMeetings.Select(m => m.Name == "UserToRemove").SingleOrDefault();

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(isThereThisUser);
        }
    }
}