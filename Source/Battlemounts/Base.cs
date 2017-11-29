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
        internal static SettingHandle<DictAnimalRecordHandler> animalSelecter;
        internal static SettingHandle<DictAnimalRecordHandler> drawSelecter;
        internal static SettingHandle<String> tabsHandler;
        internal static SettingHandle<int> accuracyPenalty;

        internal static SettingHandle<float> bodySizeFilter;
        private static Color highlight1 = new Color(0.5f, 0, 0, 0.1f);
        String[] tabNames = { "BM_tab1".Translate(), "BM_tab2".Translate()};

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
            Dictionary<String, AnimalRecord> exceptions = new Dictionary<string, AnimalRecord>();
      
            List<ThingDef> allAnimals = DefUtility.getAnimals();
            allAnimals = allAnimals.OrderBy(o => o.defName).ToList();

            accuracyPenalty = Settings.GetHandle<int>("accuracyPenalty", "BM_AccuracyPenalty_Title".Translate(), "BM_AccuracyPenalty_Description".Translate(), 10, Validators.IntRangeValidator(0, 100));

            tabsHandler = Settings.GetHandle<String>("tabs", "BM_Tabs_Title".Translate(), "", "none");
            bodySizeFilter = Settings.GetHandle<float>("bodySizeFilter", "BM_BodySizeFilter_Title".Translate(), "BM_BodySizeFilter_Description".Translate(), 0.8f);
            animalSelecter = Settings.GetHandle<DictAnimalRecordHandler>("Animalselecter", "BM_Animalselection_Title".Translate(), "BM_Animalselection_Description".Translate(), null);
            drawSelecter = Settings.GetHandle<DictAnimalRecordHandler>("drawSelecter", "BM_Drawselection_Title".Translate(), "BM_Drawselection_Description".Translate(), null);

            
            tabsHandler.CustomDrawer = rect => { return DrawUtility.CustomDrawer_Tabs(rect, tabsHandler, tabNames); };

            bodySizeFilter.CustomDrawer = rect => { return DrawUtility.CustomDrawer_Filter(rect, bodySizeFilter, false, 0, 5, highlight1); };
            animalSelecter.CustomDrawer = rect => { return DrawUtility.CustomDrawer_MatchingAnimals_active(rect, animalSelecter, allAnimals, bodySizeFilter, "BM_SizeOk".Translate(), "BM_SizeNotOk".Translate()); };
            bodySizeFilter.VisibilityPredicate = delegate { return tabsHandler.Value == tabNames[0]; };
            animalSelecter.VisibilityPredicate = delegate { return tabsHandler.Value == tabNames[0]; };


            drawSelecter.CustomDrawer = rect => { return DrawUtility.CustomDrawer_MatchingAnimals_active(rect, drawSelecter, allAnimals, null, "BM_DrawFront".Translate(), "BM_DrawBack".Translate()); };
            drawSelecter.VisibilityPredicate = delegate { return tabsHandler.Value == tabNames[1]; };

            //getPackHeights(animals);
            if(animalSelecter.Value == null)
            {
                animalSelecter.Value = getDefaultForAnimalSelecter(allAnimals);
            }
            if(drawSelecter.Value == null)
            {
                drawSelecter.Value = getDefaultForDrawSelecter(allAnimals);
            }

        }

        private DictAnimalRecordHandler getDefaultForAnimalSelecter(List<ThingDef> allAnimals)
        {
            DictAnimalRecordHandler dict = new DictAnimalRecordHandler();
            Dictionary<String, AnimalRecord> result = new Dictionary<string, AnimalRecord>();
            foreach(ThingDef animal in allAnimals)
            {
                CompProperties_Battlemounts prop = animal.GetCompProperties<CompProperties_Battlemounts>();

                float mass = animal.race.baseBodySize;
                if (prop != null && prop.isException)
                {
                    result.Add(animal.defName, new AnimalRecord(false, true));   
                }
                else
                {
                    bool shouldSelect = mass >= bodySizeFilter.Value;
                    result.Add(animal.defName, new AnimalRecord(shouldSelect, false));
                }
            }
            //result.Add("", new AnimalRecord(shouldSelect, false));
            dict.InnerList = result;
            return dict;
        }
        private DictAnimalRecordHandler getDefaultForDrawSelecter(List<ThingDef> allAnimals)
        {
            DictAnimalRecordHandler dict = new DictAnimalRecordHandler();
            Dictionary<String, AnimalRecord> result = new Dictionary<string, AnimalRecord>();
            foreach (ThingDef animal in allAnimals)
            {
                CompProperties_Battlemounts prop = animal.GetCompProperties<CompProperties_Battlemounts>();

                float mass = animal.race.baseBodySize;
                if (prop != null && prop.drawFront)
                {
                    result.Add(animal.defName, new AnimalRecord(true, true));
                }
                else
                {
                    result.Add(animal.defName, new AnimalRecord(false, false));
                }
            }
            //result.Add("", new AnimalRecord(shouldSelect, false));
            dict.InnerList = result;
            return dict;
        }

        public override void WorldLoaded()
        {
            _extendedDataStorage = UtilityWorldObjectManager.GetUtilityWorldObject<ExtendedDataStorage>();
            base.WorldLoaded();
        }

        public ExtendedDataStorage GetExtendedDataStorage()
        {
            return _extendedDataStorage;
        }
    }


}
