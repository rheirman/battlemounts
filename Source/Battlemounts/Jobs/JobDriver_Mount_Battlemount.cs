using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace Battlemounts.Jobs
{
    public class JobDriver_Mount_Battlemount : JobDriver
    {
        public override bool TryMakePreToilReservations()
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnDowned(TargetIndex.A);
            this.FailOnNotCasualInterruptible(TargetIndex.A);


            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
            yield return TalkToAnimal(TargetIndex.A);
        }

        private static Toil TalkToAnimal(TargetIndex tameeInd)
        {
            Toil toil = new Toil();
            toil.initAction = delegate
            {
                Pawn actor = toil.GetActor();
                Pawn recipient = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
                actor.interactions.TryInteractWith(recipient, InteractionDefOf.AnimalChat);
            };
            toil.defaultCompleteMode = ToilCompleteMode.Delay;
            toil.defaultDuration = 150;
            toil.AddFinishAction(delegate {
                Pawn actor = toil.GetActor();
                var extendedDataStore = Battlemounts.Instance.GetExtendedDataStorage();
                var pawnData = extendedDataStore.GetExtendedDataFor(actor);
                pawnData.mount = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
            });
            return toil;
        }
        
    }
}
