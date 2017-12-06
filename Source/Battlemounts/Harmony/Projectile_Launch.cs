using GiddyUpCore.Storage;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BattleMounts.Harmony
{

    [HarmonyPatch(typeof(Projectile), "Launch")]
    [HarmonyPatch(new Type[] { typeof(Thing), typeof(Vector3), typeof(LocalTargetInfo), typeof(Thing), typeof(Thing) })]
    static class Projectile_Launch
    {
        static void Prefix(ref Thing launcher, ref Vector3 origin)
        {
            if (!(launcher is Pawn))
            {
                return;
            }
            Pawn pawn = launcher as Pawn;
            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);

            if (pawnData.drawOffset > -1)
            {
                origin.z += pawnData.drawOffset;
            }
        }
    }
}
