using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(PawnGroupKindWorker), "GeneratePawns")]
    [HarmonyPatch(new Type[] { typeof(PawnGroupMakerParms), typeof(PawnGroupMaker), typeof(bool) })]
    static class PawnGroupKindWorker_GeneratePawns
    {
        static void Postfix(PawnGroupMakerUtility __instance, PawnGroupMakerParms parms, PawnGroupMaker groupMaker, bool errorOnZeroResults, ref List<Pawn> __result)
        {

            Map map = Find.VisibleMap;
            PawnKindDef pawnKindDef = (from a in map.Biome.AllWildAnimals
                                       where map.mapTemperature.SeasonAcceptableFor(a.race)
                                       select a).RandomElementByWeight((PawnKindDef def) => map.Biome.CommonalityOfAnimal(def));

            if (pawnKindDef == null)
            {
                Log.Error("No spawnable animals right now.");
                return;
            }
            Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, null);
            __result.Add(pawn);
        }
       
    }
}
