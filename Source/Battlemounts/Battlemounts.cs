using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HugsLib;
using HugsLib.Utils;
using Battlemounts.Storage;

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
