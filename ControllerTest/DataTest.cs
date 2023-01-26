using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    public class DataTest
    {

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void Competition_Get_IsNotNull()
        {
            // Arrange
            Data.Initialize();

            // Act
            var result = Data.competition;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Initialize_Initialization_CompetitionShouldBeFilled()
        {
            // Arrange

            // Act
            Data.Initialize();

            // Assert
            Assert.True(Data.competition.Participants.Count > 0);
            Assert.True(Data.competition.Tracks.Count > 0);
        }

        [Test]
        public void NextRace_NoCurrentRace_ShouldStart()
        {
            // Arrange
            Data.Initialize();

            // Act
            Data.NextRace();

            // Assert
            Assert.NotNull(Data.CurrentRace);
        }
    }
}
