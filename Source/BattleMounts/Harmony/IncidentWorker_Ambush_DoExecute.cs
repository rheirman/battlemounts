using Battlemounts.Utilities;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace Battlemounts.Harmony
{
        [HarmonyPatch(typeof(IncidentWorker_Ambush_EnemyFaction), "DoExecute")]
        static class IncidentWorker_Ambush_DoExecute
        {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = new List<CodeInstruction>(instructions);

            for (var i = 0; i < instructionsList.Count; i++)
            {
                CodeInstruction instruction = instructionsList[i];
                yield return instruction;
                Log.Message(instructionsList[i].opcode.ToString());
                Log.Message(instructionsList[i].operand as String);
                if(instructionsList[i].operand != null)
                {
                    Log.Message(instructionsList[i].operand.ToString());
                }

                if (instructionsList[i].operand == typeof(IncidentWorker_Ambush_EnemyFaction).GetMethod("PostProcessGeneratedPawnsAfterSpawning", new Type[] { typeof(List<Pawn>) })) //Identifier for which IL line to inject to

                {
                    //Start of injection
                    if(typeof(IncidentWorker_Ambush_EnemyFaction).GetMethod("PostProcessGeneratedPawnsAfterSpawning", new Type[] { typeof(List<Pawn>) }) == null)
                    {
                        Log.Message("method is null");
                    }
                    else
                    {
                        yield return new CodeInstruction(OpCodes.Ldloc_2);//load generated pawns as parameter
                        yield return new CodeInstruction(OpCodes.Ldarg_1);//load incidentparms as parameter
                        yield return new CodeInstruction(OpCodes.Call, typeof(NPCMountUtility).GetMethod("mountAnimals"));//Injected code
                                                                                                                          //yield return new CodeInstruction(OpCodes.Stloc_2);
                    }

                }

            }

        }
    }
}
