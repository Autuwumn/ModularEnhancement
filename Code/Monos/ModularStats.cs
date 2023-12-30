using ModsPlus;
using Photon.Pun;
using System.Linq;
using UnboundLib.Networking;
using UnboundLib;

namespace ME.Monos
{
    public class ModularStats : CardEffect
    {
        public void DoStatUpdate(int playerId, int[] newMods)
        {
            if (PhotonNetwork.IsMasterClient || PhotonNetwork.OfflineMode)
            {
                return;
            }
            NetworkingManager.RPC(typeof(ModularStats), nameof(RPC_ModularArmorStat), playerId, newMods);
        }
        [UnboundRPC]
        public static void RPC_ModularArmorStat(int playerId, int[] newMods)
        {
            var player = PlayerManager.instance.GetPlayerWithID(playerId);
            player.GetComponentInChildren<ModularArmor>().mods = newMods.ToList();
        }
    }
}