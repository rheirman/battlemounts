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

        public List<ThingDef> getMountableAnimals()
        {
            //TODO: adapt this!
            Predicate<ThingDef> isMountableAnimal = (ThingDef td) => td.category == ThingCategory.Pawn && td.race.packAnimal;
            List<ThingDef> mountableAnimals = new List<ThingDef>();
            foreach (ThingDef thingDef in from td in DefDatabase<ThingDef>.AllDefs
                                          where isMountableAnimal(td)
                                          select td)
            {
                mountableAnimals.Add(thingDef);
                Log.Message(thingDef.defName);
            }
            return mountableAnimals;
        }

    }
}
