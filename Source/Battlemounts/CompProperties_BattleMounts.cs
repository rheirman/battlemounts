using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Battlemounts
{
    class CompProperties_Battlemounts : CompProperties
    {
        public CompProperties_Battlemounts()
        {
            compClass = typeof(CompBattlemounts);
        }

        public bool drawFront = false;
        public bool isException = false;
    }
}
