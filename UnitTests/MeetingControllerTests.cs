using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visma_internship_task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visma_internship_task.Models;
using Visma_internship_task.Interfaces;
using System.Reflection;

namespace Visma_internship_task.Tests
{
    [TestClass()]
    public class MeetingControllerTests
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

        [DataRow("testName", "testResponsiblePerson", "testDescription", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01", "testName", "testResponsiblePerson")]
        [DataRow("a", "b", "c", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01", "a", "b")]
        [DataTestMethod]
        public void CreateMeetingObjectTest_shouldWork(string meetingName, string responsiblePerson, string description, Category meetingCategory, Models.Type meetingType, string meetingStart, string meetingEnd, string expectedResultName, string expectedResultPerson)
        {
            //Arrange
            DateTime startDateInput = DateTime.Parse(meetingStart);
            DateTime endDateInput = DateTime.Parse(meetingEnd);

            Meeting meetingObject = _meetingsController.CreateMeetingObject(meetingName, responsiblePerson, description, meetingCategory, meetingType, startDateInput, endDateInput);

            //Act
            var expectedName = expectedResultName;
            var expectedPerson = expectedResultPerson;

            var actualName = meetingObject.Name;
            var actualPerson = meetingObject.ResponsiblePerson;


            //Assert
            Assert.AreEqual(expectedName, actualName);
            Assert.AreEqual(expectedPerson, actualPerson);
        }

        [DataRow("", "testResponsiblePerson", "testDescription", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01")]
        [DataRow("testName", "", "testDescription", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01")]
        [DataRow("testName", "testResponsiblePerson", "", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01")]
        [DataTestMethod]
        public void CreateMeetingObjectTest_shouldFail(string meetingName, string responsiblePerson, string description, Category meetingCategory, Models.Type meetingType, string meetingStart, string meetingEnd)
        {
            //Arrange
            DateTime startDateInput = DateTime.Parse(meetingStart);
            DateTime endDateInput = DateTime.Parse(meetingEnd);



            //Assert
            Assert.ThrowsException<ArgumentException>(() => _meetingsController.CreateMeetingObject(meetingName, responsiblePerson, description, meetingCategory, meetingType, startDateInput, endDateInput));
        }

        [DataRow("testName", "testResponsiblePerson", "testDescription", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01")]
        [DataRow("a", "b", "c", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01")]
        [DataTestMethod]
        public void CreateAMeetingIfThereAreNoneTest(string meetingName, string responsiblePerson, string description, Category meetingCategory, Models.Type meetingType, string meetingStart, string meetingEnd)
        {
            //Arrange

            DateTime startDateInput = DateTime.Parse(meetingStart);
            DateTime endDateInput = DateTime.Parse(meetingEnd);
            Meeting meeting = new Meeting(meetingName, responsiblePerson, description, meetingCategory, meetingType, startDateInput, endDateInput);

            Meeting[] fullArray = new Meeting[] { meeting };


            int expectedFull = 1;

            //Act
            int actualFull = _meetingsController.CreateAMeetingIfThereAreNone(fullArray);

            //Assert
            Assert.AreEqual(actualFull, expectedFull);
            Assert.IsInstanceOfType(meeting, typeof(Meeting));
        }

        [TestMethod()]
        public void RemoveMeetingFromDBTest()
        {
            //Arrange
            Meeting meeting = new Meeting("Test", "test", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));

            _database.AddMeetingToDb(meeting);
            int numberOfMeetingBefore = _database.AllMeetings.Count;
            int numberOfMeetingAfter = _meetingsController.RemoveMeetingFromDB(meeting, _database).AllMeetings.Count;

            //Act
            bool result1 = numberOfMeetingBefore > numberOfMeetingAfter;
            var result2 = _database.AllMeetings.Where(x => x.Name == "Test");

            //Assert
            Assert.IsTrue(result1);
            Assert.AreNotEqual(meeting, result2);
        }

        
        [DataRow(1, "TestPerson", 1)]
        [DataRow(100, "Name", 100)]
        [DataTestMethod]
        public void ReturnListOfMeetingsTheUserAttendsTest(int numberOfMeetings, string person, int expected)
        {
            // Arrange
            for (int i = 0; i < numberOfMeetings; i++)
            {
                Meeting meeting = _meetingsController.CreateMeetingObject("Test" + i, person, "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
                _database.AddMeetingToDb(meeting);
            }
            List<Meeting> listOfMeetings = _meetingsController.ReturnListOfMeetingsTheUserAttends(_database, person);

            // Act
            int actual = listOfMeetings.Count();
            string actualName = listOfMeetings.Select(x => x.ResponsiblePerson).First();

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(person, actualName);
        }

        [DataRow(1, 0, 1, 15)]
        [DataRow(3, 1, 5, 10)]
        [DataRow(10, 2, 10, 3)]
        [DataTestMethod]
        public void ReturnOverlappingMeetingsTest(int numberOfMeetings, int numberOfOverlaps, int startMinutes, int duration)
        {
            // Arrange
            for (int i = 0; i < numberOfMeetings; i++)
            {

                Meeting meeting = _meetingsController.CreateMeetingObject("Test" + i, "TestPerson", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now.AddMinutes(startMinutes), DateTime.Now.AddMinutes(startMinutes + duration));
                _database.AddMeetingToDb(meeting);
                startMinutes += startMinutes;

            }
            Meeting testMeeting = _meetingsController.CreateMeetingObject("TestMeeting", "TestPerson", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now.AddMinutes(20), DateTime.Now.AddMinutes(40));
            List<Meeting> listOfMeetings = _meetingsController.ReturnListOfMeetingsTheUserAttends(_database, "TestPerson");


            // Act
            int actual = _meetingsController.ReturnOverlappingMeetings(listOfMeetings, testMeeting).Count();
            int expected = numberOfOverlaps;

            // Assert
            Assert.AreEqual(actual, expected);
        }

        

        [DataRow(",", 2, true)]
        [DataRow("a", 2, true)]
        [DataRow("()", 1, true)]
        [DataRow("1", 0, false)]
        [DataTestMethod()]
        public void ReturnMeetingsWhenDescriptionContainsTest(string input, int expected, bool expectedContains)
        {
            // Arrange 
            string[] descriptions = new string[] { "!@#$%^&*()_+,", "abcdefgh", "Lorem ipsum dolor sit amet, consectetur adipiscing elit" };
            for (int i = 0; i < descriptions.Length; i++)
            {
                Meeting meeting = _meetingsController.CreateMeetingObject("Test" + i, "TestUser", descriptions[i], Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
                _database.AddMeetingToDb(meeting);
            }

            Meeting[] meetings = _meetingsController.ReturnMeetingsWhenDescriptionContains(_database, input);


            int actual = meetings.Count();

            // Assert 
            if (meetings.Length != 0)
            {
                bool ActualContains = meetings[0].Description.Contains(input);
                Assert.AreEqual(expectedContains, ActualContains);
            }

            Assert.AreEqual(expected, actual);

        }

        [DataRow("TestUser", 2, "TestUser")]
        [DataRow("NotTestUser", 1, "NotTestUser")]
        [DataRow("1", 0, "1")]
        [DataTestMethod()]
        public void ReturnMeetingsWhenResponsibleIsTest(string input, int expected, string expectedPerson)
        {
            // Arrange
            string[] people = new string[] { "TestUser", "NotTestUser", "TestUser" };
            for (int i = 0; i < people.Length; i++)
            {
                Meeting meeting = _meetingsController.CreateMeetingObject("Test" + i, people[i], "TestDescription", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
                _database.AddMeetingToDb(meeting);
            }

            Meeting[] meetings = _meetingsController.ReturnMeetingsWhenResponsibleIs(_database, input);

            // Act
            int actual = meetings.Count();
            if (meetings.Length > 0)
            {
                string actualResponsible = meetings[0].ResponsiblePerson;
                Assert.AreEqual(expectedPerson, actualResponsible);
            }

            // Assert 
            Assert.AreEqual(expected, actual);

        }

        [DataRow(Category.CodeMonkey, 2)]
        [DataRow(Category.TeamBuilding, 1)]
        [DataRow(Category.Short, 0)]
        [DataTestMethod()]
        public void ReturnMeetingsWhenCategoryTest(Category inputCategory, int expected)
        {
            // Arrange
            Category[] categories = new Category[] { Category.CodeMonkey, Category.TeamBuilding, Category.CodeMonkey };
            for (int i = 0; i < categories.Length; i++)
            {
                Meeting meeting = _meetingsController.CreateMeetingObject("Test" + i, "TestUser", "TestDescription", categories[i], Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
                _database.AddMeetingToDb(meeting);
            }

            // Act
            Meeting[] meetings = _meetingsController.ReturnMeetingsWhenCategory(_database, inputCategory);
            int actual = meetings.Length;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}