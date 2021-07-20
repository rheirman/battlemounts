using Battlemounts.Utilities;
using BattleMounts;
using GiddyUpCore.Utilities;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(IncidentWorker_Raid), "TryExecuteWorker")]
    static class IncidentWorker_Raid_TryExecuteWorker
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = new List<CodeInstruction>(instructions);

            foreach (var instruction in instructionsList)
            {
                if (instruction.operand as MethodInfo == AccessTools.Method(typeof(PawnsArrivalModeWorker), "Arrive"))
                {
                    yield return new CodeInstruction(OpCodes.Call, typeof(IncidentWorker_Raid_TryExecuteWorker).GetMethod("MountAnimals"));//don't execute this, execute it in MountAnimals
                    continue;
                }
                yield return instruction;
            }
        }


        public static void MountAnimals(PawnsArrivalModeWorker instance, List<Pawn> pawns, IncidentParms parms)
        {
            if(pawns.Count == 0)
            {
                return;
            }
            parms.raidArrivalMode.Worker.Arrive(pawns, parms);
            if (!(parms.raidArrivalMode == null || parms.raidArrivalMode == PawnsArrivalModeDefOf.EdgeWalkIn) || (parms.raidStrategy != null && parms.raidStrategy.workerClass == typeof(RaidStrategyWorker_Siege)))
            {
                return;
            }
            NPCMountUtility.generateMounts(ref pawns, parms, Base.inBiomeWeight, Base.outBiomeWeight, Base.nonWildWeight, Base.enemyMountChance, Base.enemyMountChanceTribal);

            foreach (Pawn pawn in pawns)
            {
                if (pawn.equipment == null)
                {
                    pawn.equipment = new Pawn_EquipmentTracker(pawn);
                }
            }
            foreach(Pawn pawn in pawns)//Moved this code here so we can check if the pawn actually has apparel. 
            {
                if (pawn.apparel != null && pawn.apparel.WornApparel != null && pawn.apparel.WornApparel.Any((Apparel ap) => ap is ShieldBelt))
                {
                    LessonAutoActivator.TeachOpportunity(ConceptDefOf.ShieldBelts, OpportunityType.Critical);
                    break;
                }
            }
        }



    }
}
