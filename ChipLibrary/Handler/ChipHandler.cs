using System;
using System.Collections.Generic;

namespace ChipLibrary.Handler;

public static class ChipHandler
{
    internal static Dictionary<TechType, Type> _registeredChips = new();
    
    public static void RegisterChip<T>(TechType techType) where T : ChipBase
    {
        if (_registeredChips.ContainsKey(techType))
        {
            Main.Logger.LogWarning($"Chip for TechType {techType} is already registered. Overwriting.");
        }
        
        _registeredChips[techType] = typeof(T);
    }

    internal static void HandleEquipmentChange(Player player, string slot, InventoryItem item)
    {
        TryRemoveChips(player);

        foreach (var kvp in _registeredChips)
        {
            TechType techType = kvp.Key;
            Type chipType = kvp.Value;

            if (player.GetComponent<Inventory>()._equipment.GetCount(techType) > 0)
            {
                var component = (ChipBase)player.gameObject.AddComponent(chipType);
                component.OnEquip();
                Main.Logger.LogInfo($"Equipped chip {techType} in slot {slot}");
            }
        }
    }

    private static void TryRemoveChips(Player player)
    {
        ChipBase[] chips = player.GetComponentsInChildren<ChipBase>();
        foreach (ChipBase chip in chips)
        {
            if (chip != null)
            {
                chip.OnUnequip();
                UnityEngine.Object.Destroy(chip);
            }
        }
    }
}