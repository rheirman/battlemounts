using Battlemounts.Jobs;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(Pawn), "DrawAt")]
    [HarmonyPatch(new Type[] { typeof(Vector3), typeof(bool) })]
    static class Pawn_DrawAt
    {

        static bool Prefix(Pawn __instance, Vector3 drawLoc, bool flip = false)
        {
            //TODO: rider should have offset up instead of mount having offset down
            //TODO: determine draw position based on pack of pack animal
            var extendedDataStore = Battlemounts.Instance.GetExtendedDataStorage();
            var pawnData = extendedDataStore.GetExtendedDataFor(__instance);

            if (pawnData.mount != null)
            {
                drawLoc.z = pawnData.mount.Drawer.DrawPos.z + 0.7f;
                __instance.Drawer.DrawAt(drawLoc);
                return false;
            }
            return true;
        }
    }
}
