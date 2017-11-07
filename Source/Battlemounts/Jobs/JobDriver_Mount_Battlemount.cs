using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace Battlemounts.Jobs
{
    public class JobDriver_Mount_Battlemount : JobDriver_InteractAnimal
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            Log.Message("Jobdriver InteractAnimal MakeNewToils() called");
            return base.MakeNewToils();
        }

        protected override Toil FinalInteractToil()
        {
            Log.Message("whoohoo mounting!");
            return new Toil();
        }
    }
}
