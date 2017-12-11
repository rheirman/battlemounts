using BattleMounts;
using BattleMounts.Jobs;
using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;
using RimWorld;
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
            if(list == null)
            {
                Log.Message("list is null");
            }

            if (list.Count == 0 || parms.raidArrivalMode != PawnsArriveMode.EdgeWalkIn)
            {
                return;
            }


            Map map = (Map)parms.target;
            List<Pawn> animals = new List<Pawn>();
            int enemyMountChance = 0;
            if (parms.faction.def == FactionDefOf.Tribe)
            {
                enemyMountChance = Base.enemyMountChanceTribal;
            }
            else if (parms.faction.def != FactionDefOf.Spacer && parms.faction.def != FactionDefOf.SpacerHostile && parms.faction.def != FactionDefOf.Mechanoid)
            {
                enemyMountChance = Base.enemyMountChance;
            }
            else
            {
                return;
            }

            Random rand = new Random(DateTime.Now.Millisecond);
            foreach (Pawn pawn in list)
            {
                //TODO add chance
                int rndInt = rand.Next(1, 100);
                Log.Message("rndInt: " + rndInt);
                Log.Message("mountChance: " + enemyMountChance);
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
                    return;
                }

                Pawn animal = PawnGenerator.GeneratePawn(pawnKindDef, parms.faction);

                IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8, null);
                GenSpawn.Spawn(animal, loc, map, parms.spawnRotation, false);

                ExtendedPawnData pawnData = GiddyUpCore.Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);

                if (pawnData == null)
                {
                    Log.Message("pawndata is null");
                }
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

            //Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, null);
            //pawn.apparel = new Pawn_ApparelTracker(pawn);//have to add appareltracker to prevent null reference execption. 
            //outPawns.Add(pawn);

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
