using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Section
    {
        public SectionTypes Sectiontype;

        public Section(SectionTypes sectiontype)
        {
            Sectiontype = sectiontype;
        }
    }
    public enum SectionTypes 
    { 
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }
}
