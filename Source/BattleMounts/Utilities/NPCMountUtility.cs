using BattleMounts;
using BattleMounts.Jobs;
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
    class NPCMountUtility
    {
        public static void mountAnimals(List<Pawn> list, IncidentParms parms)
        {

            if (list.Count == 0 || !(parms.raidArrivalMode == PawnsArriveMode.EdgeWalkIn || parms.raidArrivalMode == PawnsArriveMode.Undecided) || (parms.raidStrategy != null && parms.raidStrategy.workerClass == typeof(RaidStrategyWorker_Siege)))
            {
                return;
            }

            Map map = parms.target as Map;
            if(map == null)
            {
                Caravan caravan = (Caravan)parms.target;
                int tile = caravan.Tile;
                map = Current.Game.FindMap(tile);
                if(map == null)
                {
                    return;
                }
            }
            generateMounts(list, parms, map);

        }

        private static bool generateMounts(List<Pawn> list, IncidentParms parms, Map map)
        {
            int enemyMountChance = getMountChance(parms);
            if (enemyMountChance == -1)//wrong faction
            {
                return false;
            }
            Random rand = new Random(DateTime.Now.Millisecond);
            foreach (Pawn pawn in list)
            {
                //TODO add chance
                int rndInt = rand.Next(1, 100);
                if (enemyMountChance <= rndInt)
                {
                    continue;
                }

                PawnKindDef pawnKindDef = (from a in map.Biome.AllWildAnimals
                                           where map.mapTemperature.SeasonAcceptableFor(a.race) && isMountable(a.defName)
                                           select a).RandomElementByWeight((PawnKindDef def) => map.Biome.CommonalityOfAnimal(def));

                if (pawnKindDef == null)
                {
                    Log.Error("No spawnable animals right now.");
                    return false;
                }

                Pawn animal = PawnGenerator.GeneratePawn(pawnKindDef, parms.faction);
                GenSpawn.Spawn(animal, pawn.Position, map, parms.spawnRotation, false);
                ExtendedPawnData pawnData = GiddyUpCore.Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);
                pawnData.mount = animal;
                TextureUtility.setDrawOffset(pawnData);

                if (animal.jobs == null)
                {
                    animal.jobs = new Pawn_JobTracker(animal);
                }

                Job jobAnimal = new Job(BM_JobDefOf.Mounted_BattleMount, pawn);
                jobAnimal.count = 1;
                animal.jobs.TryTakeOrderedJob(jobAnimal);

            }
            return true;
        }

        private static int getMountChance(IncidentParms parms)
        {
            if (parms.faction.def == FactionDefOf.Tribe)
            {
                return Base.enemyMountChanceTribal;
            }
            else if (parms.faction.def != FactionDefOf.Spacer && parms.faction.def != FactionDefOf.SpacerHostile && parms.faction.def != FactionDefOf.Mechanoid)
            {
                return Base.enemyMountChance;
            }
            else
            {
                return -1;
            }
        }

        //TODO: refactor this, should be in core
        private static bool isMountable(String animalName)
        {
            GiddyUpCore.AnimalRecord value;
            bool found = GiddyUpCore.Base.animalSelecter.Value.InnerList.TryGetValue(animalName, out value);
            if (found && value.isSelected)
            {
                return true;
            }
            return false;
        }
    }
}
