using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;


namespace BattleMounts.Harmony
{
    [HarmonyPatch(typeof(Pawn_JobTracker), "StartJob")]
    static class Pawn_JobTracker_StartJob
    {

        /*
        static bool Prefix(Pawn_JobTracker __instance)
        {
            if (__instance.curDriver != null && __instance.curDriver.pawn != null && __instance.curDriver.pawn.CurJob != null && __instance.curDriver.pawn.CurJob.def == GUC_JobDefOf.Mounted)
            {
                return false;
            }
            return true;
        }
        */
    }

}


