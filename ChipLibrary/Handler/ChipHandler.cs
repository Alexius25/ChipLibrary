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
    
    /// <summary>
    /// Gets all chip components of the specified type.
    /// </summary>
    /// <param name="onlyEquipped">If true, only returns equipped chips.</param>
    /// <typeparam name="T">The class type of the chip, must inherit from ChipBase</typeparam>
    /// <returns>A list of chip components of the specified type.</returns>
    public static List<T> GetChipComponent<T>(bool onlyEquipped) where T : ChipBase
    {
        var player = Player.main;
        if (player == null) return new List<T>();
        
        var chips = player.GetComponentsInChildren<T>();
        if (chips == null) return new List<T>();
        
        return onlyEquipped ? chips.Where(c => c.isEquipped).ToList() : chips.ToList();
    }
    
    /// <summary>
    /// Gets the first equipped chip of the specified type.
    /// </summary>
    public static T GetFirstEquippedChip<T>() where T : ChipBase
    {
        return GetChipComponent<T>(onlyEquipped: true).FirstOrDefault();
    }
    
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
                chip.isEquipped = false;
                Main.Logger.LogInfo($"Unequipped chip {chip.TechType} from slot {slot}");
            }
        }

        foreach (var kvp in _registeredChips)
        {
            TechType techType = kvp.Key;
            Type chipType = kvp.Value;
            
            bool alreadyEquipped = existingChips
                .Any(c => c.TechType == techType && c.isEquipped);
            
            if (!alreadyEquipped && equipment.GetCount(techType) > 0)
            {
                var component = (ChipBase)player.gameObject.GetComponent(chipType);
                
                if (component == null)
                {
                    component = (ChipBase)player.gameObject.AddComponent(chipType);
                }
                
                component.TechType = techType;
                component.isEquipped = true;
                component.OnEquip();
                OnChipEquipped?.Invoke(component);
                Main.Logger.LogInfo($"Equipped chip {techType} in slot {slot}");
            }
        }
    }
}