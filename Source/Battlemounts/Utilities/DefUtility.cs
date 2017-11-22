using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Battlemounts.Utilities
{
    public class DefUtility
    {

        public static List<PawnKindDef> getMountableAnimals()
        {
            //TODO: adapt this!
            Predicate<PawnKindDef> isMountableAnimal = (PawnKindDef d) => d.race.race.packAnimal;
            List<PawnKindDef> mountableAnimals = new List<PawnKindDef>();
            foreach (PawnKindDef thingDef in from td in DefDatabase<PawnKindDef>.AllDefs
                                          where isMountableAnimal(td)
                                          select td)
            {
                mountableAnimals.Add(thingDef);
            }
            return mountableAnimals;
        }

    }
}
