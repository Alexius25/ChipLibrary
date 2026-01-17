using ChipLibrary;
using ChipLibrary.Handler;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;

namespace TestChip;

public class TestChipMono : ChipBase
{
    public override bool UsesUpdate => false;
    
    public int equipCount = 0;

    public override void OnEquip()
    {
        base.OnEquip();
        equipCount += 1;
        ErrorMessage.AddMessage("TestChip equipped! Count: " + equipCount);
    }

    public override void OnUnequip()
    {
        base.OnUnequip();
        ErrorMessage.AddMessage("TestChip unequipped!");
    }
}

public static class TestChip
{
    public static void Register()
    {
        var prefab = new CustomPrefab("TestChip", "Test Chip",
            "A test chip for demonstration purposes.");
        prefab.SetEquipment(EquipmentType.Chip);
        prefab.SetGameObject(new CloneTemplate(prefab.Info, TechType.ComputerChip));
        prefab.Register();
        
        ChipHandler.RegisterChip<TestChipMono>(prefab.Info.TechType);
    }
}