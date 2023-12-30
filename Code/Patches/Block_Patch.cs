using HarmonyLib;
using UnityEngine;
using ME.Extensions;
using UnboundLib;

namespace ME.Patches
{
    [HarmonyPatch(typeof(Block))]
    class Block_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Block.IsBlocking))]
        static void IncreasedBlockTime(Block __instance, ref bool __result, CharacterData ___data)
        {
            if (__instance.sinceBlock < (0.3 + ___data.stats.GetAdditionalData().extraBlockTime))
            {
                __instance.ShowStatusEffectBlock();
            }

            __result = __result || __instance.sinceBlock < (0.3 + ___data.stats.GetAdditionalData().extraBlockTime);
        }
    }
}
