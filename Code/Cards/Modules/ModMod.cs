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
using System.Linq;

namespace ME.Cards
{
    public class ModModule : SimpleCard
    {
        internal static CardInfo card = null;
        public override CardDetails Details => new CardDetails
        {
            Title = "Module Expansion Component",
            Description = "<#769964>+4</color> Modular Slots",
            ModName = ME.ModInitials,
            Art = null,
            Rarity = RarityUtils.GetRarity("Mythical"),
            Theme = CardThemeColor.CardThemeColorType.TechWhite
        };
        protected override void Added(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.GetComponentInChildren<ModularArmor>().mods.Add(0);
            player.GetComponentInChildren<ModularArmor>().mods.Add(0);
            player.GetComponentInChildren<ModularArmor>().mods.Add(0);
            player.GetComponentInChildren<ModularArmor>().mods.Add(0);
        }
        protected override GameObject GetCardBase()
        {
            return ME.GetBase("ModularToken");
        }
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.categories = new CardCategory[] { ME.Module };
        }
    }
}