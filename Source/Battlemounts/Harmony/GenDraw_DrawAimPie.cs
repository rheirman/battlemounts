using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(GenDraw), "DrawAimPie")]
    static class GenDraw_DrawAimPie
    {
        /*
        static bool Prefix(GenDraw __instance, ref Thing shooter, ref LocalTargetInfo target, ref int degreesWide, ref float offsetDist)
        {
            if()
            float facing = 0f;
            if (target.Cell != shooter.Position)
            {
                if (target.Thing != null)
                {
                    facing = (target.Thing.DrawPos - shooter.Position.ToVector3Shifted()).AngleFlat();
                }
                else
                {
                    facing = (target.Cell - shooter.Position).AngleFlat;
                }
            }
            GenDraw.DrawAimPieRaw(shooter.DrawPos + new Vector3(0f, offsetDist, 0f), facing, degreesWide);
        }
        */
    }
}
