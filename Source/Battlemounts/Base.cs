using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HugsLib;
using HugsLib.Utils;
using Battlemounts.Storage;
using Battlemounts.Utilities;
using Verse;
using UnityEngine;
using HugsLib.Settings;
using RunAndGun.Utilities;

namespace Battlemounts
{
    public class Base : ModBase
    {
        private ExtendedDataStorage _extendedDataStorage;
        internal static Base Instance { get; private set; }
        internal static SettingHandle<DictAnimalRecordHandler> Animalselecter;
        internal static SettingHandle<DictAnimalRecordHandler> drawSelecter;
        internal static SettingHandle<String> tabsHandler;

        internal static SettingHandle<float> bodySizeFilter;
        private static Color highlight1 = new Color(0.5f, 0, 0, 0.1f);
        String[] tabNames = { "RM_tab1".Translate(), "RM_tab2".Translate()};


        public override string ModIdentifier
        {
            get { return "Battlemounts"; }
        }
        public Base()
        {
            Instance = this;
        }
        public override void DefsLoaded()
        {
            base.DefsLoaded();
            //tabs.Value.InnerList.Add("Select mountable animals", false);
            //tabs.Value.InnerList.Add("Configure drawing policy", false);

            List<ThingDef> allAnimals = DefUtility.getAnimals();
            allAnimals = allAnimals.OrderBy(o => o.defName).ToList();

            tabsHandler = Settings.GetHandle<String>("tabs", "RM_Tabs_Title".Translate(), "", "none");
            bodySizeFilter = Settings.GetHandle<float>("weightLimitFilter", "RM_WeightLimitFilter_Title".Translate(), "RM_WeightLimitFilter_Description".Translate(), 0.8f);
            Animalselecter = Settings.GetHandle<DictAnimalRecordHandler>("Animalselecter", "RM_Animalselection_Title".Translate(), "RM_Animalselection_Description".Translate(), null);
            drawSelecter = Settings.GetHandle<DictAnimalRecordHandler>("drawSelecter", "RM_Animalselection_Title".Translate(), "RM_Animalselection_Description".Translate(), null);


            tabsHandler.CustomDrawer = rect => { return DrawUtility.CustomDrawer_Tabs(rect, tabsHandler, tabNames); };

            bodySizeFilter.CustomDrawer = rect => { return DrawUtility.CustomDrawer_Filter(rect, bodySizeFilter, false, 0, 5, highlight1); };
            Animalselecter.CustomDrawer = rect => { return DrawUtility.CustomDrawer_MatchingAnimals_active(rect, Animalselecter, allAnimals, "RM_SizeOk".Translate(), "RM_SizeNotOk".Translate()); };
            bodySizeFilter.VisibilityPredicate = delegate { return tabsHandler.Value == tabNames[0]; };
            Animalselecter.VisibilityPredicate = delegate { return tabsHandler.Value == tabNames[1]; };


            drawSelecter.CustomDrawer = rect => { return DrawUtility.CustomDrawer_MatchingAnimals_active(rect, drawSelecter, allAnimals, "RM_DrawFront".Translate(), "RM_DrawBack".Translate()); };
            drawSelecter.VisibilityPredicate = delegate { return tabsHandler.Value == "tab2"; };

            //getPackHeights(animals);


        }


        public override void WorldLoaded()
        {
            base.WorldLoaded();
            _extendedDataStorage =
                UtilityWorldObjectManager.GetUtilityWorldObject<ExtendedDataStorage>();
        }

        public ExtendedDataStorage GetExtendedDataStorage()
        {
            return _extendedDataStorage;
        }
    }


}
