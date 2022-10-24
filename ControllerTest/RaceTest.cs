using System;
using Controller;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ControllerTest
{
    public class RaceTest
    {
        private Race race;
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
            });
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
            Assert.IsTrue(test!=null);
        }
        [Test]
        public void InitializeParticipantsStartPositions_Participants_shouldPlaceParticipantOnStart()
        {
            SectionData testS = race.GetSectionData(race.Track.Sections.First());
            Assert.IsNotNull(testS.Left);
        }
    }
}
