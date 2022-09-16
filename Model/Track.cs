using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class Track
    {
        public String Name { get; set; }
        public LinkedList<Section> Sections;

        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            for(int i = 0; i < sections.Length; i++)
            {
            this.Sections.AddLast(new Section(sections[i]));
            }
        }
    }
}
