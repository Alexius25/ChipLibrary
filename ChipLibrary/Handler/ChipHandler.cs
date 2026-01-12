using System;
using System.Collections.Generic;

namespace ChipLibrary.Handler;

public static class ChipHandler
{
    internal static Dictionary<TechType, Type> _registeredChips = new();
    
    /// <summary>
    /// Registers a custom chip type with a TechType.
    /// </summary>
    /// <param name="techType">The TechType to register the chip under</param>
    /// <typeparam name="T">The class type of the chip, must inherit from ChipBase</typeparam>
    public static void RegisterChip<T>(TechType techType) where T : ChipBase
    {
        if (techType == TechType.None)
        {
            Main.Logger.LogError($"{nameof(T)} cannot be registered with TechType.None");
            return;
        }
        
        if (_registeredChips.ContainsKey(techType))
        {
            Main.Logger.LogWarning($"Chip for TechType {techType} is already registered. Overwriting.");
        }
        
        _registeredChips[techType] = typeof(T);
        Main.Logger.LogInfo($"Registered chip: {techType} > {typeof(T).Name}");
    }

    /// <summary>
    /// Fired when any custom chip is equipped.
    /// </summary>
    public static event Action<TechType, ChipBase> OnChipEquipped;
    
    /// <summary>
    /// Fired when any custom chip is unequipped.
    /// </summary>
    public static event Action<TechType, ChipBase> OnChipUnequipped;
    
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
                OnChipEquipped?.Invoke(techType, component);
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
                OnChipUnequipped?.Invoke(chip.TechType, chip);
                chip.OnUnequip();
                UnityEngine.Object.Destroy(chip);
            }
        }
    }
}