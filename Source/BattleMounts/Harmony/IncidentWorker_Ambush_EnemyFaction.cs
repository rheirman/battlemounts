using Battlemounts.Utilities;
using BattleMounts;
using GiddyUpCore.Utilities;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(IncidentWorker_Ambush), "DoExecute")]
    static class IncidentWorker_Ambush_DoExecute
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = new List<CodeInstruction>(instructions);
            for (var i = 0; i < instructionsList.Count; i++)
            {
                CodeInstruction instruction = instructionsList[i];
                yield return instruction;

                if (instructionsList[i].operand == AccessTools.Method(typeof(IncidentWorker_Ambush), "PostProcessGeneratedPawnsAfterSpawning")) //Identifier for which IL line to inject to

                {
                    yield return new CodeInstruction(OpCodes.Ldarga_S, 2);//load generated pawns as parameter
                    yield return new CodeInstruction(OpCodes.Ldarg_1);//load incidentparms as parameter
                    yield return new CodeInstruction(OpCodes.Call, typeof(EnemyMountUtility).GetMethod("mountAnimals"));//Injected code                                                                                                                         //yield return new CodeInstruction(OpCodes.Stloc_2);
                }

            }

        }
    }
}
