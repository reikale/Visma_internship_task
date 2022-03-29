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
    public class UIToolsTests
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

        [DataRow(0, ",", "Description", "Sorry, there is no meeting with Description that includes ','")]
        [DataRow(3, ",", "Description", "Showing results of 3 meetings")]
        [DataTestMethod()]
        public void DisplayFilterResultsByPropTest(int numberOfMeetings, string userInput, string paramName, string expected)
        {
            List<Meeting> database = new List<Meeting>();
            for (int i = 0; i < numberOfMeetings; i++)
            {
                Meeting meeting = _meetingsController.CreateMeetingObject("Test" + i, "TestPerson", "responsibleTest", Category.Hub, Models.Type.Live, DateTime.Now, DateTime.Now.AddDays(1));
                database.Add(meeting);
            }
            Meeting[] result = database.ToArray();

            string actual = UITools.DisplayFilterResultsByProp(result, userInput, paramName);

            Assert.AreEqual(expected, actual);
        }
    }
}