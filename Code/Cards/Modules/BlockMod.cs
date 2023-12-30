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
    public class BlockModule : SimpleCard
    {
        internal static CardInfo card = null;
        public override CardDetails Details => new CardDetails
        {
            Title = "Block Module",
            Description = "<#769964>+0.25s</color> Block Duration",
            ModName = ME.ModInitials,
            Art = null,
            Rarity = CardInfo.Rarity.Rare,
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