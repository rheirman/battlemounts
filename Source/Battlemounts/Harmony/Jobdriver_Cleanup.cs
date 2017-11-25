using Battlemounts.Jobs;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse.AI;
namespace Battlemounts.Harmony
{
    class Jobdriver_Cleanup
    {
        [HarmonyPatch(typeof(JobDriver), "Cleanup")]
        static class JobDriver_Cleanup
        {
            static bool Prefix(JobDriver __instance)
            {
                Verse.Log.Message("cleanup!");
                /*
                if(__instance.job.def == BM_JobDefOf.Mounted_Battlemount)
                {
                    return false;
                }
                */

                return true;
            }

        }
    }
}
