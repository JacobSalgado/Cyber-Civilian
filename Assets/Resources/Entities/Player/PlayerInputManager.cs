using System;
using System.Collections;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
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

    [SerializeField] private Player _player;
    [NonSerialized] public InputType currentInputType = InputType.NONE;

    void Update()
    {
        if (currentInputType == InputType.NONE) return;

        switch (currentInputType)
        {
            case InputType.RELOAD_WEAPON:
                if (_player.currentWeapon.currentAmmo < _player.currentWeapon.maxAmmo) {
                    StartCoroutine(ReloadWeapon());
                }
                break;

            case InputType.EQUIP_WEAPON:
                EquipWeapon();
                break;

            case InputType.FIRE_WEAPON:
                FireWeapon();
                break;
        }

        
    }

    private IEnumerator ReloadWeapon()
    {
        yield return new WaitForSeconds(_player.currentWeapon.reloadTime);
        _player.currentWeapon.ReloadWeapon();
        currentInputType = InputType.NONE;
    }

    private void EquipWeapon()
    {
        /** 
        // Resets from previously having the shield
        canShieldBlock = true;
        isShieldBlocking = false;
        invincibility = false;
        */

        Player.PlayerWeaponType new_weapon_type;
        string action = _player.weaponKeybindsAction.action.activeControl.name;

        if (string.Compare(action, "q") != 0)
        {
            int num_key = int.Parse(action);
            new_weapon_type = (Player.PlayerWeaponType)(num_key - 1);
        }
        else new_weapon_type = (Player.PlayerWeaponType)(((int) _player.currentWeaponType + 1) % _player.weapons.Length);

        _player.EquipNewWeapon(new_weapon_type);
        currentInputType = InputType.NONE;
    }

    private void FireWeapon()
    {
        string fireSFX  = "";
        if (_player.currentWeaponType == Player.PlayerWeaponType.FLAMETHROWER)
            fireSFX = "FlamethrowerFire";
        else fireSFX = "PeaShooterFire";
        
        _player.ShootWeapon(_player.weapons[(int) _player.currentWeaponType], _player.fireAction, _player.firePoint, 6, fireSFX);
        currentInputType = InputType.NONE;
    }

    private void Dash()
    {
        
    }

    private void Push()
    {
        
    }
    
    private void Vortex()
    {
        
    }
}