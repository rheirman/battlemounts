using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Battlemounts.Storage
{
    public class ExtendedPawnData : IExposable
    {
        public Pawn mount = null;

        public void ExposeData()
        {
            Scribe_Values.Look(ref mount, "mount", null);
        }
    }
}
