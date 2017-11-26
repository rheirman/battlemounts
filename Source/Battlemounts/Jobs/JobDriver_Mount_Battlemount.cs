using Battlemounts.Storage;
using Battlemounts.Utilities;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
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
                var extendedDataStore = Base.Instance.GetExtendedDataStorage();
                var pawnData = extendedDataStore.GetExtendedDataFor(actor);
                pawnData.mount = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
                setDrawOffset(pawnData);
            });
            return toil;
        }

        private static void setDrawOffset(ExtendedPawnData pawnData)
        {
            //TODO: move this to a more appropriate place
                PawnKindLifeStage curKindLifeStage = pawnData.mount.ageTracker.CurKindLifeStage;
                Texture2D unreadableTexture = curKindLifeStage.bodyGraphicData.Graphic.MatSide.mainTexture as Texture2D;
                Texture2D t = TextureUtility.getReadableTexture(unreadableTexture);
                int backHeight = TextureUtility.getBackHeight(t);
                float backHeightRelative = (float)backHeight / (float)t.height;

                float textureHeight = curKindLifeStage.bodyGraphicData.drawSize.y;
                //If animal texture does not fit in a tile, take this into account
                float extraOffset = textureHeight > 1f ? (textureHeight - 1f) / 2f : 0;
                //Small extra offset, you don't want to draw pawn exactly on back
                extraOffset += (float)textureHeight / 15f;
                pawnData.drawOffset = (textureHeight * backHeightRelative - extraOffset);
                //pawnData.hasLongNeckOrHorns = TextureUtility.hasLongNeckOrHorns(t, backHeight, 6);
        }

        
    }
}
