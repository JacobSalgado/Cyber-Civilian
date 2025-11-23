using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public enum InputType
    {
        RELOAD_WEAPON,
        EQUIP_WEAPON,
        FIRE_WEAPON,
        DASH,
        PHASE,
        VORTEX,
        PUSH,
        NONE,
    }

    readonly Player _player;

    public InputType currentInputType = InputType.NONE;

    public PlayerInput(Player player) : base()
    {
        _player = player;
    }

    void Update()
    {
        if (currentInputType == InputType.NONE) return;

        switch (currentInputType)
        {
            case InputType.RELOAD_WEAPON:
                ReloadWeapon();
                break;
        }

        currentInputType = InputType.NONE;
    }

    public void ReloadWeapon()
    {
        
    }

    public void EquipWeapon()
    {
        
    }

    public void FireWeapon()
    {
        
    }

    public void Dash()
    {
        
    }

    public void Push()
    {
        
    }
    
    public void Vortex()
    {
        
    }
}