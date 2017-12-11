using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HugsLib;
using HugsLib.Utils;
using Verse;
using UnityEngine;
using HugsLib.Settings;
using RunAndGun.Utilities;
using GiddyUpCore.Storage;

namespace BattleMounts
{
    public class Base : ModBase
    {
        internal static Base Instance { get; private set; }
        internal static SettingHandle<int> accuracyPenalty;
        internal static SettingHandle<int> enemyMountChance;
        internal static SettingHandle<int> enemyMountChanceTribal;

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
            accuracyPenalty = Settings.GetHandle<int>("accuracyPenalty", "BM_AccuracyPenalty_Title".Translate(), "BM_AccuracyPenalty_Description".Translate(), 10, Validators.IntRangeValidator(minPercentage, maxPercentage));
            enemyMountChance = Settings.GetHandle<int>("enemyMountChance", "BM_EnemyMountChance_Title".Translate(), "BM_EnemyMountChance_Description".Translate(), 20, Validators.IntRangeValidator(minPercentage, maxPercentage));
            enemyMountChanceTribal = Settings.GetHandle<int>("enemyMountChanceTribal", "BM_EnemyMountChanceTribal_Title".Translate(), "BM_EnemyMountChanceTribal_Description".Translate(), 40, Validators.IntRangeValidator(minPercentage, maxPercentage));

        }

        public ExtendedDataStorage GetExtendedDataStorage()
        {
            return GiddyUpCore.Base.Instance.GetExtendedDataStorage();
        }
    }


}
