using BepInEx;
using BepInEx.Logging;

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
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}