using System;
using Controller;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Runtime.ConstrainedExecution;

namespace ControllerTest
{
    public class RaceTest
    {

        private Race race;

        // create own set of participants and tracks
        private List<IParticipant> participants;

        private IEquipment equipment;
        private Track track;

        [SetUp]
        public void SetUp()
        {
            Track track = new Track("Basic", new[]
            {
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Finish
            }, 1);
            List<IParticipant> participants = new List<IParticipant>();
            participants.Add(new Driver("Driver 1", 0, new car(10, 5, 10), TeamColors.Blue));
            participants.Add(new Driver("Driver 2", 0, new car(10, 5, 10), TeamColors.Red));
            participants.Add(new Driver("Driver 3", 0, new car(10, 5, 10), TeamColors.Yellow));
            participants.Add(new Driver("Driver 4", 0, new car(10, 5, 10), TeamColors.Green));
            participants.Add(new Driver("Driver 5", 0, new car(10, 5, 10), TeamColors.Grey));
            this.race = new Race(track, participants);
        }

        [Test]
        public void GetSectionData_Doesnt_RerunNull()
        {
            SectionData test = race.GetSectionData(race.Track.Sections.First());
            Assert.IsNotNull(test);
        }
        [Test]
        public void GetStartSections_oneInList_DoesntReturnNull()
        {
            Stack<Section> test= race.GetStartSections();
            Assert.IsNotNull(test);
        }
                [Test]
        public void GetStartSections_OneInList_OneInResult()
        {
            Stack<Section> test= race.GetStartSections();
            Assert.IsTrue(test.Count == 1);
        }
        [Test]
        public void InitializeParticipantsStartPositions_Participants_shouldPlaceParticipantOnStart()
        {
            SectionData testS = race.GetSectionData(race.Track.Sections.First());
            Assert.IsNotNull(testS.Left);
        }
        [Test]
        public void Race_Instance_ShouldNotNull()
        {
            Assert.IsNotNull(race);
        }
        [Test]
        public void GetStartGrids_OneInList_ShouldNotReturnNull()
        {
            Assert.NotNull(race.GetStartSections());
        }
        [Test]
        public void AddParticipantsPoints_FirstPlace_Adds15()
        {
            IParticipant p = new Driver("Driver", 0, new car(10, 5, 10), TeamColors.Blue);
            race.AddParticipantPoints(p, 0);
            Assert.IsTrue(p.Points == 15);
        }
        
        
        [Test]
        public void GetStartGrids_OneInList_ShouldReturnListContainingOnlyStartGrids()
        {
            // Arrange
            var startGrids = race.GetStartSections();

            // Act
            var result = startGrids.Any(x => x.Sectiontype != SectionTypes.StartGrid);

            // Assert
            Assert.That(result, Is.False);
        }

    }
}
