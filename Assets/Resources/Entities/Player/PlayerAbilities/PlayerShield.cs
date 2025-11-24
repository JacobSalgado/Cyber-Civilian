using System;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public Player _player;

    // Update is called once per frame
    void Update()
    {
        if (_player.isShielding)
        {
            if (_player.currentEnergy < 0) Shield(false);
            else _player.currentEnergy -= (int) Math.Ceiling(_player.shieldDrainRate * Time.deltaTime);
        }
    }

    public void Shield(bool activate, bool playAudio = true)
    {
        if (activate) _player.EquipShield(playAudio);
        else _player.UnequipShield(playAudio);
    }
}
