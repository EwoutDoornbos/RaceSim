using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelTest
{
    internal class Track_Test
    {
        private readonly string _name = "a name";
        private SectionTypes[] _sections;

        [SetUp]
        public void SetUp()
        {
            _sections = new[] { SectionTypes.Finish, SectionTypes.LeftCorner, SectionTypes.RightCorner };
        }

        [Test]
        public void Track_Instance_ShouldReturnObject()
        {
            Track track = new Track(_name, _sections, 2);

            Assert.That(track, Is.Not.Null);
        }

        [Test]
        public void Track_Constructor_ShouldSetName()
        {
            Track track = new Track(_name, _sections, 2);

            Assert.That(track.Name, Is.EqualTo(_name));
        }
        [Test]
        public void Track_Constructor_ShouldSetLaps()
        {
            var _Laps = 2;
            Track track = new Track(_name, _sections, _Laps);

            Assert.That(track.Laps, Is.EqualTo(_Laps));
        }

        [Test]
        public void Track_GenerateSectionsShouldReturnLinkedList()
        {
            // GenerateSections will be called by constructor
            Track track = new Track(_name, _sections, 2);

            LinkedList<Section> resultLinkedList = track.Sections;
            // convert LinkedList to array of sectionTypes to assert equal.
            SectionTypes[] resultSectionTypes = new SectionTypes[resultLinkedList.Count];
            for (int i = 0; i < resultLinkedList.Count; i++)
            {
                resultSectionTypes[i] = resultLinkedList.ElementAt(i).Sectiontype;
            }

            Assert.That(resultSectionTypes, Is.EqualTo(_sections));
        }
    }
}
