using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Track
    {
        public String Name { get; set; }
        public LinkedList<Section> Sections;
        public Track(String name, SectionTypes[] sections)
        {
            Name = name;
            Sections = SectionTypesToLinkedList(sections);
        }
        internal LinkedList<Section> SectionTypesToLinkedList(SectionTypes[] sections)
        {
            LinkedList<Section> Return = new LinkedList<Section>();

            for (int i = 0; i < sections.Length; i++)
            {
                Return.AddLast(new Section(sections[i]));
            }

            return Return;
        }
    }
}
