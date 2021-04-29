using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace SequencingRewritten
{
    [BepInDependency("com.bepis.r2api")]
    [R2APISubmoduleDependency((nameof(ArtifactAPI)))]
    [BepInPlugin("com.Wibbleh.ArtifactOfSequencingRerandomized", "Artifact of Sequencing Rerandomized", "1.0.0")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class SequencingRewritten : BaseUnityPlugin
    {

        public ArtifactDef Artifact = ScriptableObject.CreateInstance<ArtifactDef>();
        public ItemDef WhiteItem, GreenItem, RedItem, YellowItem, LunarItem;
        private static readonly Random random = new Random();
        //private Texture2D icon = LoadTexture2D(Properties.Resources.testicon);
        public ConfigEntry<int> NumWhites { get; set; }
        public ConfigEntry<int> NumGreens { get; set; }
        public ConfigEntry<int> NumReds { get; set; }
        public ConfigEntry<int> NumYellows { get; set; }
        public ConfigEntry<int> NumLunar { get; set; }

        public void Awake()
        {
            //Create settings for quantity in the config file for each item tier
            NumWhites = base.Config.Bind<int>("Quantity", "NumWhites", 1, "The number of white items to begin a run with.");
            NumGreens = base.Config.Bind<int>("Quantity", "NumGreens", 1, "The number of green items to begin a run with.");
            NumReds = base.Config.Bind<int>("Quantity", "NumReds", 1, "The number of red items to begin a run with.");
            NumYellows = base.Config.Bind<int>("Quantity", "NumYellows", 1, "The number of yellow/boss items to begin a run with.");
            NumLunar = base.Config.Bind<int>("Quantity", "NumLunar", 1, "The number of lunar items to begin a run with.");
            //TODO: Add deselect & select icons to artifact
            Artifact.nameToken = "Artifact of Resequencing";
            Artifact.descriptionToken = "Spawn with one item from every tier. Any additional items picked up will be converted into the item of their respective tier.";
            Artifact.smallIconSelectedSprite = this.Icon(Properties.Resources.testicon);
            Artifact.smallIconDeselectedSprite = this.Icon(Properties.Resources.testicon);
            ArtifactAPI.Add(Artifact);
            //Adds the items on the start of a run
            Run.onRunStartGlobal += AddBeginningItems;
        }

        //TODO: Add method to pick random item
        private ItemDef RandomItem(ItemTier tier)
        {
            ItemDef[] array = (from item in ItemCatalog.allItems
                               where Run.instance.IsItemAvailable(item) && ItemCatalog.GetItemDef(item).tier == tier
                               select ItemCatalog.GetItemDef(item)).ToArray<ItemDef>();
            return array[random.Next(0, array.Length)];
        }

        private void AddBeginningItems(Run run)
        {
            if (RunArtifactManager.instance.IsArtifactEnabled(Artifact.artifactIndex))
            {
                    Inventory.onServerItemGiven += this.ConvertItems;
                    for(int i = 0; i < CharacterMaster.readOnlyInstancesList.Count; i++)
                    {
                    WhiteItem = RandomItem((ItemTier)0);
                    GreenItem = RandomItem((ItemTier)1);
                    RedItem = RandomItem((ItemTier)2);
                    LunarItem = RandomItem((ItemTier)3);
                    YellowItem = RandomItem((ItemTier)4);
                    CharacterMaster characterMaster = CharacterMaster.readOnlyInstancesList[i];
                    characterMaster.inventory.GiveItem(WhiteItem, NumWhites.Value);
                    characterMaster.inventory.GiveItem(GreenItem, NumGreens.Value);
                    characterMaster.inventory.GiveItem(RedItem, NumReds.Value);
                    characterMaster.inventory.GiveItem(YellowItem, NumYellows.Value);
                    characterMaster.inventory.GiveItem(LunarItem, NumLunar.Value);
                    }
            }
            Inventory.onServerItemGiven -= this.ConvertItems;
        }

        //Method to convert any items that are picked up to the starting items
        private void ConvertItems(Inventory inventory, ItemIndex itemIndex, int itemCount)
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
        private ItemDef TierChooser(ItemTier tier)
        {
            switch (tier)
            {
                case 0:
                    return WhiteItem;
                case (ItemTier)1:
                    return GreenItem;
                case (ItemTier)2:
                    return RedItem;
                case (ItemTier)3:
                    return LunarItem;
                case (ItemTier)4:
                    return YellowItem;
                default:
                    return null;
            }
        }
        //Creates the sprite for the artifact's icon
        private Sprite Icon(byte[] byteData)
        {
            if (byteData == null)
                throw new ArgumentNullException("Null Argument in Icon object");
            Texture2D texture = new Texture2D(0x40, 0x40, (TextureFormat)4, false);
            ImageConversion.LoadImage(texture, byteData);
            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(.5f, .5f));
        }
    }
}
