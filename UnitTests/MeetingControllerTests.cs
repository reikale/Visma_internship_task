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
        public Database database = new Database();
        public MeetingController meetingsController = new MeetingController();

        [DataRow("testName", "testResponsiblePerson", "testDescription", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01", "testName", "testResponsiblePerson")]
        [DataRow("a", "b", "c", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01", "a", "b")]
        [DataTestMethod]
        public void CreateMeetingObjectTest_shouldWork(string meetingName, string responsiblePerson, string description, Category meetingCategory, Models.Type meetingType, string meetingStart, string meetingEnd, string expectedResultName, string expectedResultPerson)
        {
            //Arrange
            DateTime startDateInput = DateTime.Parse(meetingStart);
            DateTime endDateInput = DateTime.Parse(meetingEnd);

            Meeting meetingObject = meetingsController.CreateMeetingObject(meetingName, responsiblePerson, description, meetingCategory, meetingType, startDateInput, endDateInput);

            //Act
            var expectedName = expectedResultName;
            var expectedPerson = expectedResultPerson;

            var actualName = meetingObject.Name;
            var actualPerson = meetingObject.ResponsiblePerson;


            //Assert
            Assert.AreEqual(expectedName, actualName);
            Assert.AreEqual(expectedPerson, actualPerson);
        }

        // Ar man cia reik gaudyt exceptionus jei juos gaudys kitas metodas?
        [DataRow("", "testResponsiblePerson", "testDescription", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01")]
        [DataRow("testName", "", "testDescription", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01")]
        [DataRow("testName", "testResponsiblePerson", "", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01")]
        [DataTestMethod]
        public void CreateMeetingObjectTest_shouldFail(string meetingName, string responsiblePerson, string description, Category meetingCategory, Models.Type meetingType, string meetingStart, string meetingEnd)
        {
            //Arrange
            DateTime startDateInput = DateTime.Parse(meetingStart);
            DateTime endDateInput = DateTime.Parse(meetingEnd);

            Meeting meetingObject = meetingsController.CreateMeetingObject(meetingName, responsiblePerson, description, meetingCategory, meetingType, startDateInput, endDateInput);


            // Kodel man rodo kad  nepraeina testas nors gaunu argument exception? 
            //Assert
            Assert.ThrowsException<ArgumentException>(() => meetingsController.CreateMeetingObject(meetingName, responsiblePerson, description, meetingCategory, meetingType, startDateInput, endDateInput));
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
            //IMeeting[] emptyArray = Array.Empty<IMeeting>();


            string expectedFull = $"There are {fullArray.Length} meetings";
            //string expectedEmpty = "There are no meetings, therefore user was suggested to create one";

            //Act
            string actualFull = meetingsController.CreateAMeetingIfThereAreNone(fullArray);
            //string actualEmpty = meetingController.CreateAMeetingIfThereAreNone(emptyArray);

            //Assert
            Assert.AreEqual(actualFull, expectedFull);
            Assert.IsInstanceOfType(meeting, typeof(Meeting));
            //Assert.AreEqual(actualEmpty, actualEmpty);
        }

        [DataRow("testName", "testResponsiblePerson", "testDescription", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01", "testResponsiblePerson")]
        [DataRow("a", "b", "c", Category.TeamBuilding, Models.Type.Live, "2000-01-01", "2022-01-01", "b")]
        [DataTestMethod]
        public void CheckForResponsiblePersonTest_ShouldWork(string meetingName, string responsiblePerson, string description, Category meetingCategory, Models.Type meetingType, string meetingStart, string meetingEnd, string expectedPerson)
        {

            DateTime startDateInput = DateTime.Parse(meetingStart);
            DateTime endDateInput = DateTime.Parse(meetingEnd);
            Meeting meeting = new Meeting(meetingName, responsiblePerson, description, meetingCategory, meetingType, startDateInput, endDateInput);

            bool actualResult = meetingsController.CheckForResponsiblePerson(meeting, expectedPerson);

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

            bool actualResult = meetingsController.CheckForResponsiblePerson(meeting, expectedPerson);

            Assert.IsFalse(actualResult);
        }

        [TestMethod()]
        public void RemoveMeetingFromDBTest()
        {
            //Arrange
            Meeting meeting = new Meeting("Test", "test", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));

            database.AddMeetingToDb(meeting);
            int numberOfMeetingBefore = database.AllMeetings.Count;
            int numberOfMeetingAfter = meetingsController.RemoveMeetingFromDB(meeting, database).AllMeetings.Count;

            //Act
            bool result1 = numberOfMeetingBefore > numberOfMeetingAfter;
            var result2 = database.AllMeetings.Where(x => x.Name == "Test");

            //Assert
            Assert.IsTrue(result1);
            Assert.AreNotEqual(meeting, result2);
        }

        [DataRow("NameTrue", "NameTrue", true)]
        [DataRow("NameTrue", "NameFalse", false)]
        [DataRow("0", "0", true)]
        [DataTestMethod]
        public void CheckIfPersonAlreadyInMeetingTest(string name, string userInput, bool expected)
        {
            //Arrange
            Meeting meeting = meetingsController.CreateMeetingObject("Test", name, "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
            database.AddMeetingToDb(meeting);
            Meeting relevantMeeting = database.AllMeetings.Where(x => x.Name == "Test").SingleOrDefault();
            //Act
            bool actual = meetingsController.CheckIfPersonAlreadyInMeeting(relevantMeeting, userInput);

            //Assert
            Assert.IsTrue(expected == actual);
        }
        [DataRow(1, "TestPerson", 1)]
        [DataRow(100, "Name", 100)]
        [DataTestMethod]
        public void ReturnListOfMeetingsTheUserAttendsTest(int numberOfMeetings, string person, int expected)
        {
            // Arrange
            for (int i = 0; i < numberOfMeetings; i++)
            {
                Meeting meeting = meetingsController.CreateMeetingObject("Test" + i, person, "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
                database.AddMeetingToDb(meeting);
            }
            List<Meeting> listOfMeetings = meetingsController.ReturnListOfMeetingsTheUserAttends(database, person);

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

                Meeting meeting = meetingsController.CreateMeetingObject("Test" + i, "TestPerson", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now.AddMinutes(startMinutes), DateTime.Now.AddMinutes(startMinutes + duration));
                database.AddMeetingToDb(meeting);
                startMinutes += startMinutes;

            }
            Meeting testMeeting = meetingsController.CreateMeetingObject("TestMeeting", "TestPerson", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now.AddMinutes(20), DateTime.Now.AddMinutes(40));
            List<Meeting> listOfMeetings = meetingsController.ReturnListOfMeetingsTheUserAttends(database, "TestPerson");


            // Act
            int actual = meetingsController.ReturnOverlappingMeetings(listOfMeetings, testMeeting).Count();
            int expected = numberOfOverlaps;

            // Assert
            Assert.AreEqual(actual, expected);
        }

        [DataRow("NewUser", 2)]
        [DataTestMethod()]
        public void AddPersonToDBTest(string name, int expected)
        {
            // Arrange
            Meeting meeting = meetingsController.CreateMeetingObject("Test", "TestUser", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));

            database.AddMeetingToDb(meeting);
            Meeting relevantMeeting = database.AllMeetings.Where(x => x.Name == "Test").FirstOrDefault();


            meetingsController.AddPersonToDB(relevantMeeting, name);
            int actual = database.AllMeetings.Where(x => x.Name == "Test").FirstOrDefault().Attendees.Count;

            Assert.AreEqual(expected, actual);
        }
        [DataRow(1)]
        [DataTestMethod()]
        public void RemovePersonFromDBTest(int expected)
        {
            // Arrange
            Meeting meeting = meetingsController.CreateMeetingObject("Test", "TestUser", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));

            database.AddMeetingToDb(meeting);
            Meeting relevantMeeting = database.AllMeetings.Where(m => m.Name == "Test").SingleOrDefault();
            relevantMeeting.Attendees.Add("UserToRemove");
            meetingsController.RemovePersonFromDB(relevantMeeting, "UserToRemove");

            // Act
            int actual = relevantMeeting.Attendees.Count;
            bool isThereThisUser = database.AllMeetings.Select(m => m.Name == "UserToRemove").SingleOrDefault();

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(isThereThisUser);
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
                Meeting meeting = meetingsController.CreateMeetingObject("Test" + i, "TestUser", descriptions[i], Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
                database.AddMeetingToDb(meeting);
            }

            Meeting[] meetings = meetingsController.ReturnMeetingsWhenDescriptionContains(database, input);


            int actual = meetings.Count();

            // Assert 
            if (meetings.Length != 0)
            {
                bool ActualContains = meetings[0].Description.Contains(input);
                Assert.AreEqual(expectedContains, ActualContains);
            }

            Assert.AreEqual(expected, actual);


        }
        [DataRow(0, ",", "Sorry, there is no meeting with description that includes ','")]
        [DataRow(3, ",", "Showing results of 3 meetings")]
        [DataTestMethod()]
        public void DisplayFilterResultsDescriptionTest(int numberOfMeetings, string userInput, string expected)
        {
            List<Meeting> database = new List<Meeting>();
            for (int i = 0; i < numberOfMeetings; i++)
            {
                Meeting meeting = meetingsController.CreateMeetingObject("Test" + i, "TestPerson", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
                database.Add(meeting);
            }
            Meeting[] result = database.ToArray();

            string actual = meetingsController.DisplayFilterResultsDescription(result, userInput);

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
                Meeting meeting = meetingsController.CreateMeetingObject("Test" + i, people[i], "TestDescription", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
                database.AddMeetingToDb(meeting);
            }

            Meeting[] meetings = meetingsController.ReturnMeetingsWhenResponsibleIs(database, input);

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
                Meeting meeting = meetingsController.CreateMeetingObject("Test" + i, "TestUser", "TestDescription", categories[i], Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
                database.AddMeetingToDb(meeting);
            }

            // Act
            Meeting[] meetings = meetingsController.ReturnMeetingsWhenCategory(database, inputCategory);
            int actual = meetings.Length;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}