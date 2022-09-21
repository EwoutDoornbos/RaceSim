using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class car : IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool isBroken { get; set; }

        public car ()
        {
            Quality = 10;
            Performance = 10;
            Speed = 0;
            this.isBroken = false;
        }
    }
}
