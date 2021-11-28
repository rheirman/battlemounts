using BattleMounts;
using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace Battlemounts.Utilities
{
    class EnemyMountUtility
    {
        [SyncMethod]
        public static void mountAnimals(ref List<Pawn> list, IncidentParms parms)
        {
            if (list.Count == 0 
                || !(parms.raidArrivalMode == null 
                || parms.raidArrivalMode == PawnsArrivalModeDefOf.EdgeWalkIn)
                || parms.raidArrivalMode == BM_PawnsArrivalModeDefOf.EdgeWalkinGroups
                || (parms.raidStrategy != null && parms.raidStrategy.workerClass == typeof(RaidStrategyWorker_Siege)))
            {
                return;
            }
            NPCMountUtility.generateMounts(ref list, parms, Base.inBiomeWeight, Base.outBiomeWeight, Base.nonWildWeight, Base.enemyMountChance, Base.enemyMountChanceTribal);
            
            foreach(Pawn pawn in list)
            {
                if(pawn.equipment == null)
                {
                    pawn.equipment = new Pawn_EquipmentTracker(pawn);
                }
            }
            
        }
    }
    [DefOf]
    public static class BM_PawnsArrivalModeDefOf
    {
        public static PawnsArrivalModeDef EdgeWalkinGroups;
    }
}
