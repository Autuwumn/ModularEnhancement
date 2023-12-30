using emotitron.Utilities.Networking;
using ModsPlus;
using RarityLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using ME.Monos;
using UnityEngine;
using UnityEngine.UI;
using ME.Cards;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;

namespace ME.Cards
{
    public class ModularMK1 : CustomEffectCard<ModularArmor>
    {
        internal static CardInfo card = null;
        public override CardDetails Details => new CardDetails
        {
            Title = "Modular Armor MK1",
            Description = "Body armor that has many different attachments and attachment slots",
            ModName = ME.ModInitials,
            Art = ME.ArtAssets.LoadAsset<GameObject>("C_ModularM1"),
            Rarity = CardInfo.Rarity.Rare,
            Theme = CardThemeColor.CardThemeColorType.TechWhite,
            Stats = new[]
            {
                new CardInfoStat
                {
                    amount = "8",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Module Slots"
                },
                new CardInfoStat
                {
                    amount = "0",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Boost Slots"
                }
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        protected override void Added(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var stats = false;
            foreach(var c in player.data.currentCards)
            {
                if(c == ModStats.card)
                {
                    stats = true;
                }
            }
            if (!stats)
            {
                ME.instance.ExecuteAfterFrames(25, () =>
                {
                    ModdingUtils.Utils.Cards.instance.AddCardsToPlayer(player, new CardInfo[] { ModStats.card }, false, new string[] { "MS" }, null, null);
                });
            }
        }
        protected override GameObject GetCardBase()
        {
            return ME.GetBase("ModularBase");
        }
    }
}