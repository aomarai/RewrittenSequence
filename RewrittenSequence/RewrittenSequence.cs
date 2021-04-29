using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace RewrittenSequence
{
    [R2APISubmoduleDependency("ArtifactAPI")]
    [BepInPlugin("com.Wibbleh.RewrittenSequencing", "Artifact of Sequencing Rewritten", "1.0.0")]
    [BepInDependency("com.bepis.r2api", 1), NetworkCompatibility(1, 1)]
    public class SequencingRewritten : BaseUnityPlugin
    {

        private ArtifactDef Artifact;
        private ItemDef WhiteItem, GreenItem, RedItem, YellowItem, LunarItem;

        
        public void Awake()
        {
            //Create settings for quantity in the config file for each item tier
            NumWhites = base.Config.Bind<int>("Quantity", "NumWhites", 1, "The number of white items to begin a run with.");
            NumGreens = base.Config.Bind<int>("Quantity", "NumGreens", 1, "The number of green items to begin a run with.");
            NumReds = base.Config.Bind<int>("Quantity", "NumReds", 1, "The number of red items to begin a run with.");
            NumYellows = base.Config.Bind<int>("Quantity", "NumYellows", 1, "The number of yellow/boss items to begin a run with.");
            NumLunar = base.Config.Bind<int>("Quantity", "NumLunar", 1, "The number of lunar items to begin a run with.");
            //TODO: Add deselect & select icons to artifact
            ArtifactAPI.Add("Artifact of Resequencing", "Begin each run with one item from every tier. Any items picked up will be converted into the starting item of that tier.",
                "Artifact of Resequencing", );
            //TODO: Implement adding randomized items on run
            Run.onRunStartGlobal();
        }

        //TODO: Add method to pick random item
        private ItemDef RandomItem(ItemTier itemTier)
        {
            
        }

        //Method to convert any items that are picked up to the starting items
        private void ConvertItems(Inventory inventory, ItemIndex itemIndex)
        {
            ItemDef itemDef = this.TierChooser(ItemCatalog.GetItemDef(itemIndex).tier);
            if(itemDef != null && itemDef.itemIndex != itemIndex)
            {
                int quantity = inventory.GetItemCount(itemIndex);
                inventory.RemoveItem(itemIndex, quantity);
                inventory.GiveItem(itemDef, quantity);
            }
        }

        //Assigns the item tier identifiers to the variables in this DLL
        private ItemDef TierChooser(ItemTier itemTier)
        {
            switch (itemTier)
            {
                case 0:
                    return WhiteItem;
                case 1:
                    return GreenItem;
                case 2:
                    return RedItem;
                case 3:
                    return LunarItem;
                case 4:
                    return YellowItem;
            }
            return null;
        }
        //Creates the sprite for the artifact's icon
        private Sprite Icon(byte[] byteData)
        {
            if (byteData == null)
                throw new ArgumentNullException("Null Argument in Icon object");
            Texture2D texture = new Texture2D(0x40, 0x40, 4, false);
            ImageConversion.LoadImage(texture, byteData);
            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(.5f, .5f));
        }

        public ConfigEntry<int> NumWhites { get; set; }

        public ConfigEntry<int> NumGreens { get; set; }

        public ConfigEntry<int> NumReds { get; set; }

        public ConfigEntry<int> NumYellows { get; set; }

        public ConfigEntry<int> NumLunar { get; set; }
    }
}
