using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }
        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }
        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track track = new Track("Track");
            _competition.Tracks.Enqueue(track);
            var result = _competition.NextTrack();
            Assert.AreEqual(track, result);
        }
        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            _competition.Tracks.Enqueue(new Track("track"));
            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }
        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track track = new Track("track");
            Track track2 = new Track("track2");
            _competition.Tracks.Enqueue(track);
            _competition.Tracks.Enqueue(track2);
            var result = _competition.NextTrack();
            var result2 = _competition.NextTrack();
            Assert.AreEqual(track, result);
            Assert.AreEqual(track2, result2);
        }
    }
}
