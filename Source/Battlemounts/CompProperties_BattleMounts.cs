using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BattleMounts
{
    class CompProperties_BattleMounts : CompProperties
    {
        public CompProperties_BattleMounts()
        {
            compClass = typeof(CompBattleMounts);
        }

        public bool drawFront = false;
        public bool isException = false;
    }
}
