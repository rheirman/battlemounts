using GiddyUpCore.Storage;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using GiddyUpCore.Utilities;

//LEGACY CODE: This is included to ensure compatibility with old saves, up to date files have been moved to Giddy-up Core. 


namespace BattleMounts.Jobs
{
    public class JobDriver_Mount_BattleMount : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        private Pawn Mount { get { return job.targetA.Thing as Pawn; } }

        protected override IEnumerable<Toil> MakeNewToils()
        {

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnDowned(TargetIndex.A);

            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
            yield return TalkToAnimal(TargetIndex.A);
        }

        private Toil TalkToAnimal(TargetIndex tameeInd)
        {
            Toil toil = new Toil();

            toil.AddFailCondition(delegate { return Mount.CurJob.def != BM_JobDefOf.Mounted_BattleMount; });
            toil.initAction = delegate
            {
                Pawn actor = toil.GetActor();
                actor.interactions.TryInteractWith(Mount, InteractionDefOf.AnimalChat);
            };
            toil.defaultCompleteMode = ToilCompleteMode.Delay;
            toil.defaultDuration = 150;
            toil.AddFinishAction(delegate {
                if (Mount.CurJob != null && Mount.CurJob.def == BM_JobDefOf.Mounted_BattleMount)
                {
                    Pawn actor = toil.GetActor();
                    ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(actor);
                    pawnData.mount = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
                    TextureUtility.setDrawOffset(pawnData);
                }
            });
            return toil;
        }



        
    }
}
