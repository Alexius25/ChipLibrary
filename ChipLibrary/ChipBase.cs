using UnityEngine;

namespace ChipLibrary;

public class ChipBase : MonoBehaviour
{
    public bool isEquipped = false;
    
    public virtual void OnEquip()
    {
        isEquipped = true;
    }
    
    public virtual void OnUnequip()
    {
        isEquipped = false;
    }
    
    public virtual void EquippedUpdate()
    {
    }

    private void Update()
    {
        if (isEquipped)
        {
            EquippedUpdate();
        }
    }
}