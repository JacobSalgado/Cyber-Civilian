using UnityEngine;

public class PlayerPhase : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int phaseDrainRate = 50;

    // Update is called once per frame
    void Update()
    {
        if (_player.isPhasing)
        {
            if (_player.currentEnergy <= 0) Phase(false);
            else _player.currentEnergy -= Mathf.RoundToInt(phaseDrainRate * Time.deltaTime);
        }
    }

    public void Phase(bool activate, bool playAudio = true)
    {
        if (activate)
        {
            if (playAudio) _player.audioManager.PlayAudioSource("PhaseStart");
            _player.isPhasing = true;
            _player.ChangeSpriteAlpha(_player.spriteRenderer, 0.35f);
            _player.ChangeSpriteAlpha(_player.weaponRenderer, 0.35f);
            _player.gameObject.layer = 3;
            //Debug.Log("Change state to phasing");
        }
        else
        {
            if (playAudio) _player.audioManager.PlayAudioSource("PhaseEnd");
            _player.isPhasing = false;
            _player.ChangeSpriteAlpha(_player.spriteRenderer, 1f);
            _player.ChangeSpriteAlpha(_player.weaponRenderer, 1f);
            _player.gameObject.layer = 6;
            //Debug.Log("Change state to normal");
        }
    }

    public bool CanActivate()
    {
        return _player.currentEnergy - phaseDrainRate >= 0;
    }
}
