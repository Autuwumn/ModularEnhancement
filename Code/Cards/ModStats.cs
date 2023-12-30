using ME.Cards;
using ModdingUtils.MonoBehaviours;
using ModsPlus;
using RarityLib.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;
using UnboundLib.Networking;
using ME.Monos;
using Photon.Pun;
using UnboundLib.Utils;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using System.Net;
using ModdingUtils.Extensions;

namespace ME.Monos
{
    public class ModStats : CustomEffectCard<ModularStats>
    {
        internal static CardInfo card = null;
        public override CardDetails Details => new CardDetails
        {
            Title = "Modular Stats",
            Description = "Displays Modular Stats\nM to open Modular Settings",
            ModName = ME.ModInitials,
            Art = null,
            Rarity = CardInfo.Rarity.Rare,
            Theme = CardThemeColor.CardThemeColorType.TechWhite,
        };
        protected override GameObject GetCardBase()
        {
            return ME.GetBase("StatsBase");
        }
        public override bool GetEnabled()
        {
            return false;
        }
        protected override void Added(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            ME.instance.ExecuteAfterFrames(25, () =>
            {
                player.data.stats.GetAdditionalData().blacklistedCategories.Remove(ME.Module);
            });
        }
        protected override void Removed(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            ME.instance.ExecuteAfterFrames(25, () =>
            {
                player.data.stats.GetAdditionalData().blacklistedCategories.Add(ME.Module);
            });
        }
    }
}

