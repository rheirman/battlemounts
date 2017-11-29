using BattleMounts.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace BattleMounts.Jobs
{
    //TODO: find better solution for riderData so I don't have to assign each time it is used. 
    //TODO: find a way to get rid of shouldEnd
    class JobDriver_Mounted_Battlemount : JobDriver
    {
        private Pawn Rider { get { return job.targetA.Thing as Pawn; } }
        ExtendedPawnData riderData;
        bool shouldEnd = false;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return waitForRider();
            yield return delegateMovement();
        }
        public override bool TryMakePreToilReservations()
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

            if (Rider.Downed || Rider.Dead)
            {
                //Log.Message("cancel job, rider downed or dead");
                ReadyForNextToil();
                return true;
            }
            if (!Rider.Drafted || Rider.InMentalState || pawn.InMentalState)
            {
                //Log.Message("cancel job, rider or mount in mental state");
                riderData.mount = null;
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
                if(Rider.CurJob.def != BM_JobDefOf.Mount_Battlemount && riderData.mount == null){
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

            toil.initAction = delegate
            {
                riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                bool shouldCancel = cancelJobIfNeeded(riderData);
                if (shouldCancel)
                {
                    return;
                }
                pawn.Drawer.tweener = Rider.Drawer.tweener;
                pawn.Position = Rider.Position;

            };
            toil.tickAction  = delegate
            {
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
                Log.Message("finishing mounted action");

                riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                riderData.reset();
                pawn.Drawer.tweener = new PawnTweener(pawn);
            });

            return toil;

        }
    }


}
