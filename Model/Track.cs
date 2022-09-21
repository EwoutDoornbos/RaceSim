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
        public Track(String name)
        {
            Name = name;
        }
    }
}
