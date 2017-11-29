using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BattleMounts.Harmony
{
    [HarmonyPatch(typeof(VerbProperties), "AdjustedAccuracy")]
    static class VerbProperties_AdjustedAccuracy
    {
        static void Postfix(VerbProperties __instance, ref Thing equipment, ref float __result)
        {

            if (equipment == null || equipment.holdingOwner == null || !(equipment.holdingOwner.Owner is Pawn_EquipmentTracker))
            {
                return;
            }
            if (equipment == null || equipment.holdingOwner == null || equipment.holdingOwner.Owner == null)
            {
                return;
            }
            Pawn_EquipmentTracker eqt = (Pawn_EquipmentTracker)equipment.holdingOwner.Owner;
            Pawn pawn = Traverse.Create(eqt).Field("pawn").GetValue<Pawn>();
            if (pawn == null || pawn.stances == null)
            {
                return;
            }
            if (Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn).mount == null)
            {
                return;
            }
            float factor = ((float)(100 - Base.accuracyPenalty.Value) / 100);
            __result *= factor;
        }
    }
}
