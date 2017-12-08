using BattleMounts;
using BattleMounts.Jobs;
using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(PawnGroupKindWorker_Normal), "GeneratePawns")]
    [HarmonyPatch(new Type[] { typeof(PawnGroupMakerParms), typeof(PawnGroupMaker), typeof(List <Pawn>), typeof(bool) })]
    static class PawnGroupKindWorker_Normal_GeneratePawns
    {
        static void Postfix(PawnGroupMakerUtility __instance, PawnGroupMakerParms parms, PawnGroupMaker groupMaker, ref List<Pawn> outPawns, bool errorOnZeroResults)
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
            Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, parms.faction);
            pawn.apparel = new Pawn_ApparelTracker(pawn);//have to add appareltracker to prevent null reference execption. 
            outPawns.Add(pawn);

            if (outPawns[0] != null)
            {
                ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(outPawns[0]);
                if(pawnData == null)
                {
                    Log.Message("pawndata is null");
                }
                pawnData.mount = outPawns[1];
                TextureUtility.setDrawOffset(pawnData);
            }
            if (outPawns[1].jobs == null)
            {
                outPawns[1].jobs = new Pawn_JobTracker(outPawns[1]);
                Log.Message("animal.jobs is null");
            }

            Job jobAnimal = new Job(BM_JobDefOf.Mounted_BattleMount, outPawns[0]);
            jobAnimal.count = 1;
            outPawns[1].jobs.debugLog = true;
            outPawns[1].jobs.TryTakeOrderedJob(jobAnimal);

        }
       
    }
}
