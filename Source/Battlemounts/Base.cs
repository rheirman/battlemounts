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
            accuracyPenalty = Settings.GetHandle<int>("accuracyPenalty", "BM_AccuracyPenalty_Title".Translate(), "BM_AccuracyPenalty_Description".Translate(), 10, Validators.IntRangeValidator(0, 100));
        }

        public ExtendedDataStorage GetExtendedDataStorage()
        {
            return GiddyUpCore.Base.Instance.GetExtendedDataStorage();
        }
    }


}
