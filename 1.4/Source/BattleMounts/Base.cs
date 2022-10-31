using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HugsLib;
using HugsLib.Utils;
using Verse;
using UnityEngine;
using HugsLib.Settings;
using GiddyUpCore.Storage;
using RimWorld;
using Battlemounts.Concepts;

namespace BattleMounts
{
    public class Base : ModBase
    {
        internal static Base Instance { get; private set; }

        internal static SettingHandle<int> enemyMountChance;
        internal static SettingHandle<int> enemyMountChanceTribal;

        internal static SettingHandle<int> inBiomeWeight;
        internal static SettingHandle<int> outBiomeWeight;
        internal static SettingHandle<int> nonWildWeight;

        private int minPercentage = 0;
        private int maxPercentage = 100;

        public override string ModIdentifier
        {
            get { return "BattleMounts"; }
        }
        public Base()
        {
            Instance = this;
        }
        public override void DefsLoaded()
        {
            base.DefsLoaded();

            enemyMountChance = Settings.GetHandle<int>("enemyMountChance", "BM_EnemyMountChance_Title".Translate(), "BM_EnemyMountChance_Description".Translate(), 20, Validators.IntRangeValidator(minPercentage, maxPercentage));
            enemyMountChanceTribal = Settings.GetHandle<int>("enemyMountChanceTribal", "BM_EnemyMountChanceTribal_Title".Translate(), "BM_EnemyMountChanceTribal_Description".Translate(), 40, Validators.IntRangeValidator(minPercentage, maxPercentage));

            inBiomeWeight = Settings.GetHandle<int>("inBiomeWeight", "BM_InBiomeWeight_Title".Translate(), "BM_InBiomeWeight_Description".Translate(), 70, Validators.IntRangeValidator(minPercentage, maxPercentage));
            outBiomeWeight = Settings.GetHandle<int>("outBiomeWeight", "BM_OutBiomeWeight_Title".Translate(), "BM_OutBiomeWeight_Description".Translate(), 15, Validators.IntRangeValidator(minPercentage, maxPercentage));
            nonWildWeight = Settings.GetHandle<int>("nonWildWeight", "BM_NonWildWeight_Title".Translate(), "BM_NonWildWeight_Description".Translate(), 15, Validators.IntRangeValidator(minPercentage, maxPercentage));


        }
        public override void WorldLoaded()
        {
            base.WorldLoaded();
            LessonAutoActivator.TeachOpportunity(BM_ConceptDefOf.BM_Mounting, OpportunityType.GoodToKnow);
            LessonAutoActivator.TeachOpportunity(BM_ConceptDefOf.BM_Enemy_Mounting, OpportunityType.GoodToKnow);
        }

        public ExtendedDataStorage GetExtendedDataStorage()
        {
            return GiddyUpCore.Base.Instance.GetExtendedDataStorage();
        }
    }


}
