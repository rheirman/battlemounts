using Battlemounts.Jobs;
using Battlemounts.Storage;
using Harmony;
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
            ExtendedPawnData pawnData = Battlemounts.Instance.GetExtendedDataStorage().GetExtendedDataFor(__instance);

            if (pawnData.mount != null)
            {
                drawLoc = pawnData.mount.Drawer.DrawPos;

                if (pawnData.drawOffset != -1)
                {
                    drawLoc.z = pawnData.mount.Drawer.DrawPos.z + pawnData.drawOffset;
                }
                __instance.Drawer.DrawAt(drawLoc);
                return false;
            }
            return true;
        }


    }
}
