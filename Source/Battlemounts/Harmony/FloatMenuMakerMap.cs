using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using BattleMounts.Jobs;
using GiddyUpCore;

namespace BattleMounts.Harmony
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

                if (pawnData.mount == null )
                {
                    AnimalRecord value;
                    bool found = GiddyUpCore.Base.animalSelecter.Value.InnerList.TryGetValue(animal.def.defName, out value);
                    if (found && !value.isSelected)
                    {
                        opts.Add(new FloatMenuOption("BM_NotInModOptions".Translate(), null, MenuOptionPriority.Default));
                        return;
                    }

                    if (!(animal.CurJob.def == JobDefOf.Wait ||
                        animal.CurJob.def == JobDefOf.Goto ||
                        animal.CurJob.def == JobDefOf.GotoWander ||
                        animal.CurJob.def == JobDefOf.WaitWander ||
                        animal.CurJob.def == JobDefOf.WaitMaintainPosture ||
                        animal.CurJob.def == JobDefOf.WaitSafeTemperature ||
                        animal.CurJob.def == JobDefOf.GotoSafeTemperature ||
                        animal.CurJob.def == JobDefOf.LayDown))
                    {
                        opts.Add(new FloatMenuOption("BM_AnimalBusy".Translate(), null, MenuOptionPriority.Default));
                        return;
                    }
                    if (animal.ageTracker.CurLifeStageIndex != animal.RaceProps.lifeStageAges.Count - 1)
                    {
                        opts.Add(new FloatMenuOption("BM_NotFullyGrown".Translate(), null, MenuOptionPriority.Default));
                        return;
                    }
                    if(!(animal.training != null && animal.training.IsCompleted(TrainableDefOf.Obedience)))
                    {
                        opts.Add(new FloatMenuOption("BM_NeedsObedience".Translate(), null, MenuOptionPriority.Default));
                        return;
                    }



                    Action action = delegate
                    {

                        Job jobRider = new Job(BM_JobDefOf.Mount_BattleMount, animal);
                        jobRider.count = 1;
                        pawn.jobs.TryTakeOrderedJob(jobRider);
                        animal.jobs.StopAll();
                        animal.pather.StopDead();
                        Job jobAnimal = new Job(BM_JobDefOf.Mounted_BattleMount, pawn);
                        jobAnimal.count = 1;
                        animal.jobs.TryTakeOrderedJob(jobAnimal);
                    };
                    opts.Add(new FloatMenuOption("BM_Mount".Translate(), action, MenuOptionPriority.Default));

                }
                else if(animal == pawnData.mount)
                {
                    if(opts.Count > 0 ) opts.RemoveAt(0);//Remove option to attack your own mount

                    Action action = delegate
                    {
                        pawnData.reset();
                    };
                    opts.Add(new FloatMenuOption("BM_Dismount".Translate(), action, MenuOptionPriority.Default));

                }


            }
        }
    }
}
