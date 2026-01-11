using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace TestChip;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Main : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;
        
        TestChip.Register();
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}