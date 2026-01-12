using BepInEx;
using BepInEx.Logging;
using ChipLibrary;
using ChipLibrary.Handler;

namespace TestChip;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.alexius25.chiplibrary")]
public class Main : BaseUnityPlugin
{
    private new static ManualLogSource Logger { get; set; }

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;
        
        TestChip.Register();
        
        ChipHandler.OnChipEquipped += OnChipEquipped;
        ChipHandler.OnChipUnequipped += OnChipUnequipped;
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void OnChipEquipped(TechType techType, ChipBase chip)
    {
        ErrorMessage.AddMessage($"Equipped Chip: {techType}");
    }
    
    private void OnChipUnequipped(TechType techType, ChipBase chip)
    {
        ErrorMessage.AddMessage($"Unequipped Chip: {techType}");
    }
}