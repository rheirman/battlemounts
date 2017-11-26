using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Battlemounts.Jobs;
namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddDraftedOrders")]
    [HarmonyPatch(new Type[] { typeof(Vector3), typeof(Pawn), typeof(List<FloatMenuOption>) })]
    static class FloatMenuMakerMap_AddDraftedOrders
    {
        static void Postfix(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
        {
            foreach (LocalTargetInfo current in GenUI.TargetsAt(clickPos, TargetingParameters.ForAttackHostile(), true))
            {
                if (!(current.Thing is Pawn) || !((Pawn)current.Thing).RaceProps.Animal)
                {
                    return;
                }

                var pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);
                Pawn animal = (Pawn)current.Thing;

                

                if (pawnData.mount == null && animal.ageTracker.CurLifeStageIndex == animal.RaceProps.lifeStageAges.Count - 1 && animal.training != null && animal.training.IsCompleted(TrainableDefOf.Obedience))
                {
                    if (!(animal.CurJob.def == JobDefOf.Wait ||
                        animal.CurJob.def == JobDefOf.Goto ||
                        animal.CurJob.def == JobDefOf.GotoWander ||
                        animal.CurJob.def == JobDefOf.WaitWander ||
                        animal.CurJob.def == JobDefOf.WaitMaintainPosture ||
                        animal.CurJob.def == JobDefOf.WaitSafeTemperature ||
                        animal.CurJob.def == JobDefOf.GotoSafeTemperature ||
                        animal.CurJob.def == JobDefOf.LayDown))
                    {
                        opts.Add(new FloatMenuOption("Cannot mount, animal busy", null, MenuOptionPriority.Default));
                        return;
                    }

                    Action action = delegate
                    {

                        Job jobRider = new Job(BM_JobDefOf.Mount_Battlemount, animal);
                        jobRider.count = 1;
                        pawn.jobs.TryTakeOrderedJob(jobRider);
                        animal.jobs.StopAll();
                        animal.pather.StopDead();
                        Job jobAnimal = new Job(BM_JobDefOf.Mounted_Battlemount, pawn);
                        jobAnimal.count = 1;
                        animal.jobs.TryTakeOrderedJob(jobAnimal);
                    };
                    opts.Add(new FloatMenuOption("Mount", action, MenuOptionPriority.Default));

                }
                else if(animal == pawnData.mount)
                {
                    Action action = delegate
                    {
                        pawnData.reset();
                    };
                    opts.Add(new FloatMenuOption("Dismount", action, MenuOptionPriority.Default));

                }


            }
        }
    }
}
