using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
namespace BattleMounts.Harmony
{
    class Jobdriver_Cleanup
    {
        [HarmonyPatch(typeof(JobDriver), "Cleanup")]
        static class JobDriver_Cleanup
        {
            static void Prefix(JobDriver __instance)
            {
                if(__instance.job.def != GUC_JobDefOf.Mounted)
                {
                    return;
                }
                JobDriver_Mounted jobDriver = (JobDriver_Mounted) __instance;

                ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(jobDriver.pawn);
                if (pawnData != null)
                {
                    Pawn Rider = jobDriver.Rider;
                    ExtendedPawnData riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                    riderData.reset();
                    jobDriver.pawn.Drawer.tweener = new PawnTweener(jobDriver.pawn);
                }
            }

        }
    }
}
