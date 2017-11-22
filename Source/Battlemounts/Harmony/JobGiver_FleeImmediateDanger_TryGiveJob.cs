using Battlemounts.Jobs;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;


namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(RimWorld.JobGiver_FleeImmediateDanger), "TryGiveJob")]
    static class JobGiver_FleeImmediateDanger_TryGiveJob
    {
        static bool PreFix(RimWorld.JobGiver_FleeImmediateDanger __instance, ref Pawn pawn)
        {
            Log.Message("calling trygivejob");
            if (pawn.CurJob.def == BM_JobDefOf.Mounted_Battlemount)
            {
                Log.Message("ignoring trygivejob");

                return false;
            }
            return true;
        }

    }
}

