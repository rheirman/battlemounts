using Battlemounts.Jobs;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;


namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(Pawn_JobTracker), "StartJob")]
    static class Pawn_Mindstate_StartFleeingBecauseOfPawnAction
    {

        static bool Prefix(Pawn_JobTracker __instance)
        {
            Log.Message("calling StartJob");
            if (__instance.curDriver!= null && __instance.curDriver.pawn != null && __instance.curDriver.pawn.CurJob != null && __instance.curDriver.pawn.CurJob.def == BM_JobDefOf.Mounted_Battlemount)
            {
                Log.Message("ignoring StartJob");

                return false;
            }
            return true;
        }
    }
}

