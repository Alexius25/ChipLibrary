using HarmonyLib;
using UnityEngine;

namespace ChipLibrary.Patches;

[HarmonyPatch(typeof(Player))]
public static class PlayerPatches
{
    private static GameObject _playerObject;
    private static Player _player;

    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    public static void Player_Awake_Postfix(Player __instance)
    {
        _playerObject = __instance.gameObject;
        _player = __instance;
    }

    [HarmonyPatch("EquipmentChanged")]
    [HarmonyPostfix]
    public static void Player_EquipmentChanged_Postfix(string slot, InventoryItem item)
    {
        if (_player != null)
        {
            Handler.ChipHandler.HandleEquipmentChange(_player, slot, item);
        }
    }
}