using HarmonyLib;
using UnityEngine;
using ME.Extensions;
using UnboundLib;

namespace ME.Extensions
{
    public static class BlockExtension
    {
        public static void UpdateParticleDuration(this Block block)
        {
            var ratio = 1f / ((0.3f + block.GetComponent<CharacterStatModifiers>().GetAdditionalData().extraBlockTime) / 0.3f);

            var main = block.particle.main;
            main.simulationSpeed = ratio;
        }

    }
}
