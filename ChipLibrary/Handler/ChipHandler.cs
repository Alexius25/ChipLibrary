using System;
using System.Linq;
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
    public static event Action<ChipBase> OnChipEquipped;
    
    /// <summary>
    /// Fired when any custom chip is unequipped.
    /// </summary>
    public static event Action<ChipBase> OnChipUnequipped;
    
    internal static void HandleEquipmentChange(Player player, string slot, InventoryItem item)
    {
        var equipment = player.GetComponent<Inventory>()._equipment;
        if (equipment == null) return;
        
        var existingChips = player.GetComponentsInChildren<ChipBase>();
        if (existingChips == null) return;

        foreach (var chip in player.GetComponentsInChildren<ChipBase>())
        {
            if (chip != null && equipment.GetCount(chip.TechType) == 0)
            {
                OnChipUnequipped?.Invoke(chip);
                chip.OnUnequip();
                UnityEngine.Object.Destroy(chip);
                Main.Logger.LogInfo($"Unequipped chip {chip.TechType} from slot {slot}");
            }
        }

        foreach (var kvp in _registeredChips)
        {
            TechType techType = kvp.Key;
            Type chipType = kvp.Value;
            
            bool alreadyEquipped = existingChips
                .Any(c => c.TechType == techType);
            
            if (!alreadyEquipped && equipment.GetCount(techType) > 0)
            {
                var component = (ChipBase)player.gameObject.AddComponent(chipType);
                component.TechType = techType;
                component.OnEquip();
                OnChipEquipped?.Invoke(component);
                Main.Logger.LogInfo($"Equipped chip {techType} in slot {slot}");
            }
        }
    }
}