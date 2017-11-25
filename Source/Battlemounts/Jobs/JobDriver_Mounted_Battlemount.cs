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
            if (!Rider.Drafted || Rider.InMentalState || pawn.InMentalState)
            {
                riderData.mount = null;
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

            toil.FailOn(() => Rider.CurJob.def != BM_JobDefOf.Mount_Battlemount && riderData.mount == null);

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
            ExtendedPawnData riderData;
            Toil toil = new Toil();
            toil.defaultCompleteMode = ToilCompleteMode.Never;

            toil.initAction = delegate
            {
                pawn.Drawer.tweener = Rider.Drawer.tweener;
                pawn.Position = Rider.Position;

            };
            toil.tickAction  = delegate
            {
                riderData = Battlemounts.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                pawn.Drawer.tweener = Rider.Drawer.tweener;
                cancelJobIfNeeded(riderData);
                pawn.Position = Rider.Position;
                pawn.Rotation = Rider.Rotation;

            };

            toil.AddFinishAction(delegate {
                Log.Message("finishing mounted action");

                riderData = Battlemounts.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                riderData.mount = null;
                pawn.Drawer.tweener = new PawnTweener(pawn);
            });

            return toil;

        }
    }


}
