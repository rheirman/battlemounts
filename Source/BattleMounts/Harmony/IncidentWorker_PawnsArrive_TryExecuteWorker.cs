using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Battlemounts.Harmony
{
    [HarmonyPatch(typeof(IncidentWorker_PawnsArrive), "TryExecuteWorker")]
    static class IncidentWorker_PawnsArrive_TryExecuteWorker
    {
        static void Postfix(IncidentWorker_PawnsArrive __instance, ref IncidentParms parms)
        {
            Map map = (Map) parms.target;


            foreach (Thing thing in from thing in map.spawnedThings
                                    where (thing is Pawn && (Pawn)thing.Faction != null && (Pawn)thing.Faction == )
                                    select thing) ;
        }
    }
}
