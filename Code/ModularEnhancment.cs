using BepInEx;
using HarmonyLib;
using Jotunn.Utils;
using ME.Cards;
using ME.Monos;
using ModsPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;
using RarityLib.Utils;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using ModdingUtils.MonoBehaviours;
using System.Numerics;
using UnityEngine.Experimental.PlayerLoop;
using System.Net.Http.Headers;
using TMPro;
using UnboundLib;
using WillsWackyManagers.Utils;

namespace ME
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.rarity.lib", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.cardtheme.lib", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.CrazyCoders.Rounds.RarityBundle", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.willis.rounds.modsplus", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class ME : BaseUnityPlugin
    {
        private const string ModId = "koala.modular.enhancement";
        private const string ModName = "Modular Enhancement";
        public const string Version = "0.0.1";
        public const string ModInitials = "MODLR";

        public static ME instance;

        internal static AssetBundle ArtAssets;

        internal static ObjectsToSpawn wallbounce;

        public static CardInfo[] Modules;

        internal static CardCategory Module;

        void Start()
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
            instance = this;

            var fieldInfo = typeof(UnboundLib.Utils.CardManager).GetField("defaultCards", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            CardInfo[] vanillaCards = (CardInfo[])fieldInfo.GetValue(null);
            CardInfo mayhemCard = vanillaCards.Where((c) => c.cardName.ToLower() == "mayhem").ToArray()[0];
            wallbounce = mayhemCard.gameObject.GetComponent<Gun>().objectsToSpawn[0];

            ArtAssets = AssetUtils.LoadAssetBundleFromResources("modularassets", typeof(ME).Assembly);
            if(ArtAssets == null) UnityEngine.Debug.Log("Modular Enhancment art asset bundle either doesn't exist or failed to load.");

            Module = CustomCardCategories.instance.CardCategory("Module");

            GameModeManager.AddHook(GameModeHooks.HookGameStart, GameStart);
            ME.instance.ExecuteAfterFrames(10, () =>
            {
                var exclusiveCards = new List<CardInfo>() { ModularMK1.card, ModularMK2.card, ModularMK3.card };
                ME.Modules = new CardInfo[] { BlankCard.card, BuffModule.card, DamageModule.card, HealthModule.card, MobilityModule.card, BlockModule.card };
                foreach (var i in exclusiveCards) foreach (var j in exclusiveCards) if (i != j) CustomCardCategories.instance.MakeCardsExclusive(i, j);
                /**
                CustomCardCategories.instance.MakeCardsExclusive(ModularMK1.card, ModularMK2.card);
                CustomCardCategories.instance.MakeCardsExclusive(ModularMK1.card, ModularMK3.card);
                CustomCardCategories.instance.MakeCardsExclusive(ModularMK2.card, ModularMK1.card);
                CustomCardCategories.instance.MakeCardsExclusive(ModularMK2.card, ModularMK3.card);
                CustomCardCategories.instance.MakeCardsExclusive(ModularMK3.card, ModularMK1.card);
                CustomCardCategories.instance.MakeCardsExclusive(ModularMK3.card, ModularMK2.card);
                **/
            });
        }
        void Awake()
        {
            CustomCard.BuildCard<ModularMK1>((card) => { ModularMK1.card = card; card.SetAbbreviation("M1"); });
            CustomCard.BuildCard<ModularMK2>((card) => { ModularMK2.card = card; card.SetAbbreviation("M2"); });
            CustomCard.BuildCard<ModularMK3>((card) => { ModularMK3.card = card; card.SetAbbreviation("M3"); });

            CustomCard.BuildCard<DamageModule>((card) => { DamageModule.card = card; card.SetAbbreviation("DM"); });
            CustomCard.BuildCard<HealthModule>((card) => { HealthModule.card = card; card.SetAbbreviation("HM"); });
            CustomCard.BuildCard<MobilityModule>((card) => { MobilityModule.card = card; card.SetAbbreviation("MM"); });
            CustomCard.BuildCard<BuffModule>((card) => { BuffModule.card = card; card.SetAbbreviation("BM"); });
            CustomCard.BuildCard<BlockModule>((card) => { BlockModule.card = card; card.SetAbbreviation("BM"); });

            CustomCard.BuildCard<ModModule>((card) => { ModModule.card = card; card.SetAbbreviation("MC"); });

            CustomCard.BuildCard<ModStats>((card) => { ModStats.card = card; ModdingUtils.Utils.Cards.instance.AddHiddenCard(card); });


            //RarityUtils.AddRarity("ModStats", 0.1f, new Color(0.8f, 0.3f, 0f), new Color(0.4f, 0.15f, 0f));
        }
        IEnumerator GameStart(IGameModeHandler gm)
        {
            foreach (var player in PlayerManager.instance.players)
            {
                if (!ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).blacklistedCategories.Contains(Module))
                {
                    ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).blacklistedCategories.Add(Module);
                }
            }
                //ModularMK1.card.rarity = CardInfo.Rarity.Rare;
                //ModularMK2.card.rarity = RarityUtils.GetRarity("Legendary");
                //ModularMK3.card.rarity = RarityUtils.GetRarity("Mythical");
                yield return null;
        }
        public static GameObject GetBase(string chosen)
        {
            GameObject MCB = ArtAssets.LoadAsset<GameObject>(chosen);
            MCB.transform.GetChild(0).GetComponent<Canvas>().sortingLayerID = -1160774667;
            return MCB;
        }
    }
}
