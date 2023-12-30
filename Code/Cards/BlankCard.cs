using ModsPlus;
using RarityLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ME.Cards
{
    public class BlankCard : SimpleCard
    {
        internal static CardInfo card = null;
        public override CardDetails Details => new CardDetails
        {
            Title = "Blank",
            Description = "Does Nothing",
            ModName = ME.ModInitials,
            Art = ME.ArtAssets.LoadAsset<GameObject>("C_Blank"),
            Rarity = RarityUtils.GetRarity("Legendary"),
            Theme = CardThemeColor.CardThemeColorType.TechWhite,
            Stats = new[]
            {
                new CardInfoStat
                {
                    amount = "+100%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Nothing"
                }
            }
        };
        protected override GameObject GetCardBase()
        {
            GameObject MCB = ME.ArtAssets.LoadAsset<GameObject>("ModularBase");
            MCB.transform.GetChild(0).GetComponent<Canvas>().sortingLayerID = -1160774667;
            return MCB;
        }
    }
}
