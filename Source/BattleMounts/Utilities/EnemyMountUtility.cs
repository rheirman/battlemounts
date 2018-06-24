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

namespace Battlemounts.Utilities
{
    class EnemyMountUtility
    {
        public static void mountAnimals(ref List<Pawn> list, IncidentParms parms)
        {
            if (list.Count == 0 || !(parms.raidArrivalMode == null || parms.raidArrivalMode == PawnsArrivalModeDefOf.EdgeWalkIn) || (parms.raidStrategy != null && parms.raidStrategy.workerClass == typeof(RaidStrategyWorker_Siege)))
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
}
