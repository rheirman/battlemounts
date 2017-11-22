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
        public bool hasLongNeckOrHorns = false;
        public float drawOffset = -1;

        public void ExposeData()
        {
            Scribe_Values.Look(ref mount, "mount", null);
            Scribe_Values.Look(ref drawOffset, "drawOffset", 0);

        }
    }
}
