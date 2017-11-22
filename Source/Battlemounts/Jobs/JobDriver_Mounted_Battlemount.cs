using Battlemounts.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Battlemounts.Jobs
{
    class JobDriver_Mounted_Battlemount : JobDriver
    {
        private Pawn Rider { get { return job.targetA.Thing as Pawn; } }
        private bool isMounted;


        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return waitForRider();
            yield return delegateMovement();
        }
        public override bool TryMakePreToilReservations()
        {
            return true;
        }

        private void cancelJobIfNeeded(ExtendedPawnData riderData)
        {
            if (Rider.Downed || Rider.Dead)
            {
                ReadyForNextToil();
                return;
            }
            if (riderData.mount == null)
            {
                ReadyForNextToil();
            }

        }

        private Toil waitForRider()
        {
            ExtendedPawnData riderData = Battlemounts.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);

            Toil toil = new Toil();

            toil.defaultCompleteMode = ToilCompleteMode.Never;

            toil.tickAction = delegate
            {
                if (riderData.mount != null)
                {
                    ReadyForNextToil();
                }
            };
            return toil;
        }

        private Toil delegateMovement()
        {
            ExtendedPawnData riderData = Battlemounts.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
            Toil toil = new Toil();
            toil.defaultCompleteMode = ToilCompleteMode.Never;

            toil.initAction = delegate
            {
                Log.Message("initAction called");
                pawn.Drawer.tweener = Rider.Drawer.tweener;
                pawn.Position = Rider.Position;

                //pawn.Drawer.rotator = new PawnRotator(pawn);
                //pawn.Drawer.rotator.FaceCell(Rider.pather.nextCell);

            };
            //toil.tickAction();
            toil.tickAction  = delegate
            {
                cancelJobIfNeeded(riderData);

                Log.Message("tickAction called");

                pawn.Position = Rider.Position;
                pawn.Rotation = Rider.Rotation;


                //pawn.Drawer.rotator.PawnRotatorTick();
            };

            toil.AddFinishAction(delegate {
                Log.Message("finishAction called!");
                riderData.mount = null;
                pawn.Drawer.tweener = new PawnTweener(pawn);
            });

            //toil.FailOnDespawnedOrNull(TargetIndex.A);
            return toil;

        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.isMounted, "isMounted", false, false);
            //Scribe_Values.Look<Job>(ref job, "job", null, false);
        }

    }


}
