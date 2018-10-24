using Battlemounts.Utilities;
using BattleMounts;
using GiddyUpCore.Utilities;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
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

            for (var i = 0; i < instructionsList.Count; i++)
            {
                CodeInstruction instruction = instructionsList[i];

                if(instruction.operand == AccessTools.Method(typeof(PawnsArrivalModeWorker), "Arrive"))
                {
                    yield return new CodeInstruction(OpCodes.Call, typeof(IncidentWorker_Raid_TryExecuteWorker).GetMethod("DoNothing"));//don't execute this, execute it in MountAnimals
                    continue;
                }
                if (instruction.operand == AccessTools.Method(typeof(PawnGroupMakerUtility), "GeneratePawns")) //Identifier for which IL line to inject to
                {
                    //Start of injection
                    yield return new CodeInstruction(OpCodes.Ldarg_1);//load incidentparms as parameter
                    yield return new CodeInstruction(OpCodes.Call, typeof(IncidentWorker_Raid_TryExecuteWorker).GetMethod("MountAnimals"));//replace GeneratePawns by custom code
                }
                else if (instructionsList[i].operand == AccessTools.Method(typeof(PlayerKnowledgeDatabase), "IsComplete")) //Prevent teaching about shieldbelts for animals
                {
                    yield return new CodeInstruction(OpCodes.Call, typeof(IncidentWorker_Raid_TryExecuteWorker).GetMethod("ReturnTrue"));//Injected code
                }
                else
                {                          
                    yield return instruction;

                }

            }
        }

        public static bool ReturnTrue(ConceptDef conc)
        {
            return true;
        }
        public static void DoNothing(List<Pawn> pawns, IncidentParms parms)
        {
            //do nothing
        }
        public static IEnumerable<Pawn> MountAnimals(PawnGroupMakerParms groupParms, bool warnOnZeroResults, IncidentParms parms)
        {
            List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(groupParms, true).ToList();
            if(list.Count == 0)
            {
                return list;
            }
            parms.raidArrivalMode.Worker.Arrive(list, parms);
            if (!(parms.raidArrivalMode == null || parms.raidArrivalMode == PawnsArrivalModeDefOf.EdgeWalkIn) || (parms.raidStrategy != null && parms.raidStrategy.workerClass == typeof(RaidStrategyWorker_Siege)))
            {
                return list;
            }
            NPCMountUtility.generateMounts(ref list, parms, Base.inBiomeWeight, Base.outBiomeWeight, Base.nonWildWeight, Base.enemyMountChance, Base.enemyMountChanceTribal);

            foreach (Pawn pawn in list)
            {
                if (pawn.equipment == null)
                {
                    pawn.equipment = new Pawn_EquipmentTracker(pawn);
                }
            }
            foreach(Pawn pawn in list)//Moved this code here so we can check if the pawn actually has apparel. 
            {
                if (pawn.apparel != null && pawn.apparel.WornApparel != null && pawn.apparel.WornApparel.Any((Apparel ap) => ap is ShieldBelt))
                {
                    LessonAutoActivator.TeachOpportunity(ConceptDefOf.ShieldBelts, OpportunityType.Critical);
                    break;
                }
            }
            return list;
        }



    }
}
