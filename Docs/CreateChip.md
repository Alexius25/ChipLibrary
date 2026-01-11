# Chip Creation Guide

## Step 1
Make a class that inherits from `ChipBase`.cs:
```csharp
public class TestChipMono : ChipBase
{
    public override void OnEquip()
    {
        // Code to execute when the chip is equipped
    }

    public override void OnUnequip()
    {
        // Code to execute when the chip is unequipped
    }
}
```

Notes:
- The `ChipBase` class itself inherits from `MonoBehaviour`, so you can use all Unity lifecycle methods (e.g., `Start`, `Update`) in your chip class.
- Override the `OnEquip` and `OnUnequip` methods to define behavior when the chip is equipped or unequipped.

## Step 2
Create your item prefab:
```csharp
public static class TestChip
{
    public static void Register()
    {
        var prefab = new CustomPrefab("TestChip", "Test Chip",
            "A test chip for demonstration purposes.");
        prefab.SetEquipment(EquipmentType.Chip);
        prefab.SetGameObject(new CloneTemplate(prefab.Info, TechType.ComputerChip));
        prefab.Register();

        // Register the Class you created previously with the techtype of the item
        ChipHandler.RegisterChip<TestChipMono>(prefab.Info.TechType);
    }
}
```

Note:
- You can put these two snippets in the same file if you prefer.

## Step 3
Register your chip during the mod initialization:
```csharp
using BepInEx;
using BepInEx.Logging;

namespace TestChip;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.alexius25.chiplibrary")]
public class Main : BaseUnityPlugin
{
    private new static ManualLogSource Logger { get; private set; }

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;
        
        TestChip.Register();
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}
```

Note:
- You need to add a dependency on ChipLibrary in your BepInEx plugin attributes. (`[BepInDependency("com.alexius25.chiplibrary")]`)

## Final Notes
- An [Example](https://github.com/Alexius25/ChipLibrary/tree/master/TestChip) mod is available on GitHub for reference.
- Available [Debug Commands](./DebugCommands.md) can help you test your chip in-game.
