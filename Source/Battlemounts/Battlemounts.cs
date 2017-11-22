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

namespace Battlemounts
{
    public class Battlemounts : ModBase
    {
        private ExtendedDataStorage _extendedDataStorage;
        internal static Battlemounts Instance { get; private set; }

        public override string ModIdentifier
        {
            get { return "Battlemounts"; }
        }
        public Battlemounts()
        {
            Instance = this;
        }
        public override void DefsLoaded()
        {
            base.DefsLoaded();

            List<PawnKindDef> animals = DefUtility.getMountableAnimals();
            //getPackHeights(animals);


        }


        //TODO: this doesn't work
        private static void getPackHeights(List<PawnKindDef> animals)
        {
            foreach (PawnKindDef animal in animals)
            {
                Log.Message(animal.defName);
                PawnKindLifeStage lastStage = animal.lifeStages.Last();
                Graphic g = lastStage.bodyGraphicData.Graphic;
                Log.Message("g.path" + g.path);
                
                Texture2D t = GraphicDatabase.Get<Graphic_Multi>(lastStage.bodyGraphicData + "Pack", ShaderDatabase.Cutout, lastStage.bodyGraphicData.drawSize, Color.white).MatSingle.mainTexture as Texture2D;
                for (int i = 0; i < t.height; i++)
                {
                    for (int j = 0; j < t.width; j++)
                    {
                        Color c = t.GetPixel(j, i);
                        Log.Message("r: " + c.r + ", g: " + c.g + ", b: " + c.b);
                    }
                }
                

            }
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
