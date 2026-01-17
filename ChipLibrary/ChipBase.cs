using UnityEngine;

namespace ChipLibrary;

/// <summary>
/// The base class for every custom chip. Inherits from MonoBehaviour.
/// </summary>
public class ChipBase : MonoBehaviour
{
    /// <summary>
    /// Whether this chip is currently equipped or not.
    /// </summary>
    public bool isEquipped = false;
    
    /// <summary>
    /// The TechType of this chip. Automatically set when registered.
    /// </summary>
    public TechType TechType = TechType.None;
    
    /// <summary>
    /// Whether this chip uses the EquippedUpdate method.
    /// </summary>
    public virtual bool UsesUpdate => true;
    
    /// <summary>
    /// Code to run when the chip is equipped.
    /// </summary>
    public virtual void OnEquip()
    {
        isEquipped = true;
    }
    
    /// <summary>
    /// Code to run when the chip is unequipped.
    /// </summary>
    public virtual void OnUnequip()
    {
        isEquipped = false;
    }
    
    /// <summary>
    /// Code to run every frame while the chip is equipped, if UsesUpdate is true.
    /// </summary>
    public virtual void EquippedUpdate()
    {
    }

    private void Update()
    {
        if (isEquipped && UsesUpdate)
        {
            EquippedUpdate();
        }
    }
}