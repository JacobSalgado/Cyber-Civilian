using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public enum InputType
    {
        RELOAD_WEAPON,
        EQUIP_WEAPON,
        FIRE_WEAPON,
        PHASE,
        SHIELD,
        VORTEX,
        NONE,
    }

    [SerializeField] private Player _player;
    public InputType currentInputType = InputType.NONE;
    [NonSerialized] public bool CanReload = false;
    [NonSerialized] public bool CanEquip = false;
    [NonSerialized] public bool CanFire = false;

    void Update()
    {
        //print(currentInputType);
        UpdateInputConditions();

        if (currentInputType == InputType.NONE) return;

        // NOTE: for all player abilities except Dash, turn off/stop other abilities first
        // ability use should be mutually-exclusive (if you perform one, you can't do the others)

        switch (currentInputType)
        {
            case InputType.RELOAD_WEAPON:
                StartCoroutine(ReloadWeapon());
                currentInputType = InputType.NONE;
                break;

            case InputType.EQUIP_WEAPON:
                if (_player.isShielding) _player.shieldAbility.Shield(false, false);
                else if (_player.isVortexing) _player.vortexAbility.EmitVortex(false, false);

                EquipWeapon();
                break;

            case InputType.FIRE_WEAPON:
                if (_player.isVortexing) _player.vortexAbility.EmitVortex();

                FireWeapon();
                break;
            
            case InputType.PHASE:
                if (_player.isShielding) _player.shieldAbility.Shield(false, false);
                else if (_player.isVortexing) _player.vortexAbility.EmitVortex(false, false);

                if ((_player.currentEnergy - _player.phaseDrainRate) >= 0)
                    _player.phaseAbility.Phase(!_player.isPhasing);

                currentInputType = InputType.NONE;
                break;
            
            case InputType.SHIELD:
                if (_player.isPhasing) _player.phaseAbility.Phase(false, false);
                else if (_player.isVortexing) _player.vortexAbility.EmitVortex(false, false);

                if ((_player.currentEnergy - _player.shieldDrainRate) >= 0)
                    _player.shieldAbility.Shield(!_player.isShielding);

                currentInputType = InputType.NONE;
                break;

            case InputType.VORTEX:
                if (_player.isShielding) _player.shieldAbility.Shield(false, false);
                else if (_player.isPhasing) _player.phaseAbility.Phase(false, false);

                if ((_player.currentEnergy - _player.vortexDrainRate) >= 0)
                    _player.vortexAbility.EmitVortex();

                currentInputType = InputType.NONE;
                break;
        }        
    }

    private void UpdateInputConditions()
    {
        CanReload = _player.currentWeapon.currentAmmo < _player.currentWeapon.maxAmmo &&
        !_player.isVortexing;

        CanEquip = !_player.isShielding &&
        !_player.isVortexing;

        CanFire = (_player.currentWeapon.currentAmmo - _player.currentWeapon.ammoCost) >= 0 &&
        !_player.isShielding &&
        !_player.isReloading;
    }

    private IEnumerator ReloadWeapon()
    {
        //Debug.Log("Reload Started");
        _player.isReloading = true;
        _player.chargeMeter.TurnOnMeter(ChargeMeter.MeterType.RELOADING_WEAPON, 0f, _player.currentWeapon.reloadTime);

        yield return new WaitForSeconds(_player.currentWeapon.reloadTime);

        _player.chargeMeter.TurnOffMeter();
        _player.currentWeapon.ReloadWeapon();
        _player.isReloading = false;
    }

    private void EquipWeapon()
    {
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
}