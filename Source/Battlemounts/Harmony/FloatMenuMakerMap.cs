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
                Pawn animal = (Pawn) current.Thing;
                Action action = delegate
                {

                    Job job = new Job(BM_JobDefOf.Mount_Battlemount, animal);
                    job.count = 1;
                    pawn.jobs.TryTakeOrderedJob(job);
                };

                opts.Add(new FloatMenuOption("Use as battlemount", action, MenuOptionPriority.Default));
            }
        }
    }
}
