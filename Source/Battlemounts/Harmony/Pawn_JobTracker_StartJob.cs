﻿using BattleMounts.Jobs;
using Harmony;
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

        static bool Prefix(Pawn_JobTracker __instance)
        {
            if (__instance.curDriver!= null && __instance.curDriver.pawn != null && __instance.curDriver.pawn.CurJob != null && __instance.curDriver.pawn.CurJob.def == BM_JobDefOf.Mounted_BattleMount)
            {
                return false;
            }
            return true;
        }
    }
}

