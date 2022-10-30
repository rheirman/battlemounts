﻿using GiddyUpCore.Storage;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

//LEGACY CODE: This is included to ensure compatibility with old saves, up to date files have been moved to Giddy-up Core. 

namespace BattleMounts.Jobs
{
    //TODO: find better solution for riderData so I don't have to assign each time it is used. 
    //TODO: find a way to get rid of shouldEnd
    class JobDriver_Mounted_BattleMount : JobDriver
    {
        public Pawn Rider { get { return job.targetA.Thing as Pawn; } }
        ExtendedPawnData riderData;
        bool shouldEnd = false;
        bool isFinished = false;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return waitForRider();
            yield return delegateMovement();
        }
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        private bool cancelJobIfNeeded(ExtendedPawnData riderData)
        {

            if (shouldEnd)
            {
                //Log.Message("cancel job, shouldEnd called");
                ReadyForNextToil();
                return true;
            }

            Thing thing = pawn as Thing;
            if (Rider.Downed || Rider.Dead || pawn.Downed || pawn.Dead || pawn.IsBurning() || Rider.IsBurning())
            {
               //Log.Message("cancel job, rider downed or dead");
                ReadyForNextToil();
                return true;
            }
            if (pawn.InMentalState || (Rider.InMentalState && Rider.MentalState.def != MentalStateDefOf.PanicFlee))
            {
                //Log.Message("cancel job, rider or mount in mental state");
                ReadyForNextToil();
                return true;
            }
            if (!Rider.Spawned)
            {
                pawn.DeSpawn();
                ReadyForNextToil();
                return true;
            }
            if (!Rider.Drafted && Rider.IsColonist)
            {
                //Log.Message("cancel job, rider not drafted while being colonist");
                ReadyForNextToil();
                return true;
            }
            if (riderData.mount == null)
            {
                //Log.Message("cancel job, rider has no mount");
                ReadyForNextToil();
                return true;
            }
            return false;

        }

        private Toil waitForRider()
        {
            Toil toil = new Toil();

            toil.defaultCompleteMode = ToilCompleteMode.Never;

            toil.tickAction = delegate
            {
                riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                if (riderData.mount != null && riderData.mount == pawn)
                {
                    ReadyForNextToil();
                }
                if(Rider.CurJob.def != BM_JobDefOf.Mount_BattleMount && riderData.mount == null){
                    shouldEnd = true;
                    ReadyForNextToil();
                }

            };
            return toil;
        }

        

        private Toil delegateMovement()
        {
            Toil toil = new Toil();
            toil.defaultCompleteMode = ToilCompleteMode.Never;

            toil.tickAction  = delegate
            {
                if (isFinished)
                {
                    return;
                }
                riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                bool shouldCancel = cancelJobIfNeeded(riderData);
                if (shouldCancel)
                {
                    return;
                }
                pawn.Drawer.tweener = Rider.Drawer.tweener;

                pawn.Position = Rider.Position;
                pawn.Rotation = Rider.Rotation;
                pawn.meleeVerbs.TryMeleeAttack(Rider.TargetCurrentlyAimingAt.Thing, this.job.verbToUse, false);

            };

            toil.AddFinishAction(delegate {
                if (!Rider.IsColonist)
                {
                    if(pawn.Faction != null)
                    {
                        pawn.SetFaction(null);
                    }
                }
                isFinished = true;
                riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                riderData.reset();
                pawn.Drawer.tweener = new PawnTweener(pawn);
                //pawn.Position = Rider.Position;
            });

            return toil;

        }
    }


}
