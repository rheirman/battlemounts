using Battlemounts.Utilities;
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
                yield return instruction;
                //Log.Message(instructionsList[i].opcode.ToString());
                //Log.Message(instructionsList[i].operand as String);

                if (instructionsList[i].operand == AccessTools.Method(typeof(IncidentWorker_Raid), "GetLetterLabel")) //Identifier for which IL line to inject to
                {
                    //Start of injection
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 4);//load generated pawns as parameter
                    yield return new CodeInstruction(OpCodes.Ldarg_1);//load incidentparms as parameter
                    yield return new CodeInstruction(OpCodes.Call, typeof(EnemyMountUtility).GetMethod("mountAnimals"));//Injected code
                }

                if (i > 0 && instructionsList[i].operand == AccessTools.Method(typeof(RaidStrategyWorker), "MakeLords")) //Identifier for which IL line to inject to
                {
                    //Start of injection
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 4);//load generated pawns as parameter
                    yield return new CodeInstruction(OpCodes.Call, typeof(IncidentWorker_Raid_TryExecuteWorker).GetMethod("removeAnimals"));//Injected code
                }

            }


        }

        public static void removeAnimals(ref List<Pawn> pawns)
        {
            List<Pawn> animals = new List<Pawn>();
            foreach (Pawn pawn in pawns)
            {
                if (pawn.RaceProps.Animal)
                {
                    animals.Add(pawn);
                }
            }
            pawns = pawns.Except(animals).ToList();
        }


    }
}
