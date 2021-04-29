using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace RetwrittenSequence
{
    [BepInDependency("com.bepis.r2api", 1), BepInPlugin("com.Wibbleh.RewrittenSequencing", "Artifact of Sequencing Rewritten", "1.0.0"), NetworkCompatibility(1, 1), R2APISubmoduleDependency(new string[] { "ArtifactAPI" })]
    [R2APISubmoduleDependency("ArtifactAPI")];
    public class SequencingRewritten : BaseUnityPlugin
    {

        ArtifactDef Artifact;
        ItemDef WhiteItem, GreenItem, RedItem, YellowItem, LunarItem;

        
        public void Awake()
        {
            //Create settings for quantity in the config file
            NumWhites = base.Config.Bind<int>("Quantity", "NumWhites", 1, "The number of white items to begin a run with.");
            NumGreens = base.Config.Bind<int>("Quantity", "NumGreens", 1, "The number of green items to begin a run with.");
            NumReds = base.Config.Bind<int>("Quantity", "NumReds", 1, "The number of red items to begin a run with.");
            NumYellows = base.Config.Bind<int>("Quantity", "NumYellows", 1, "The number of yellow/boss items to begin a run with.");
            NumLunar = base.Config.Bind<int>("Quantity", "NumLunar", 1, "The number of lunar items to begin a run with.");
            ArtifactAPI.Add()
        }

        private ItemDef RandomItem(ItemTier itemTier)
        {

        }

        public ConfigEntry<int> NumWhites { get; set; }

        public ConfigEntry<int> NumGreens { get; set; }

        public ConfigEntry<int> NumReds { get; set; }

        public ConfigEntry<int> NumYellows { get; set; }

        public ConfigEntry<int> NumLunar { get; set; }
    }
}
