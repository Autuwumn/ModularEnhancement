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

namespace ME.Cards
{
    public class DamageModule : SimpleCard
    {
        internal static CardInfo card = null;
        public override CardDetails Details => new CardDetails
        {
            Title = "Damage Module",
            Description = "<#769964>+30%</color> Damage",
            ModName = ME.ModInitials,
            Art = null,
            Rarity = CardInfo.Rarity.Uncommon,
            Theme = CardThemeColor.CardThemeColorType.TechWhite
        };
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