using Model;

namespace ModelTest
{
    public class SectionTest
    {
        [Test]
        public void Section_Object_NotNull()
        {
            Section section = new Section(SectionTypes.Straight);

            Assert.That(section, Is.Not.Null);
        }

        [Test]
        public void Section_SectionType_ShouldReturnEqual()
        {
            Section section = new Section(SectionTypes.Straight);

            var actual = section.Sectiontype;

            Assert.That(actual, Is.EqualTo(SectionTypes.Straight));
        }

        [Test]
        public void Section_SectionType_SetSection_ShouldReturnEqual()
        {
            Section section = new Section(SectionTypes.Straight);

            section.Sectiontype = SectionTypes.Finish;
            var actual = section.Sectiontype;

            Assert.That(actual, Is.EqualTo(SectionTypes.Finish));
        }
    }
}