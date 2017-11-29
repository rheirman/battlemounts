using BattleMounts.Jobs;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse.AI;
namespace BattleMounts.Harmony
{
    class Jobdriver_Cleanup
    {
        [HarmonyPatch(typeof(JobDriver), "Cleanup")]
        static class JobDriver_Cleanup
        {
            static bool Prefix(JobDriver __instance)
            {
                
                if(__instance.job.def == BM_JobDefOf.Mounted_BattleMount)
                {
                    Verse.Log.Message("ignoring cleanup!");

                    return false;
                }
                

                return true;
            }

        }
    }
}
