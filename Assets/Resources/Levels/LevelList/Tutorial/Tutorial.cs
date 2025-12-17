using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : Level
{
    public enum ShowcaseSection
    {
        CONTROLS,
        PEASHOOTER,
        RAILGUN,
        MISSILE,
        PLASMA,
        FLAMETHROWER,
        DASH,
        PHASE, 
        SHIELD,
        VORTEX,
        PRACTICE,
    };


    [SerializeField] ShowcaseSection section = ShowcaseSection.CONTROLS;
    [SerializeField] InputActionReference nextSectionAction;

    void Update()
    {
        if (!LevelManager.current_level.hasStarted) return;

        CheckSectionCompletion();
    }

    private void CheckSectionCompletion()
    {
        if (nextSectionAction.action.WasPressedThisFrame())
        {
            if (section == ShowcaseSection.PRACTICE){
                LevelManager.isLevelCompleted = true;
                return;
            }
        
            section = (ShowcaseSection) ((int) section + 1);
            SetupSection();
        }
    }

    public void SetupSection()
    {
        switch (section)
        {
            case ShowcaseSection.CONTROLS:
                player.hud.SetMissionText("Controls");
                player.hud.SetLevelObjectiveText("Press Y to move on to next section.");

                player.hud.SetTutorialText("Use WASD to move around.  Use the mouse to aim and press left mouse button to shoot. Use scroll wheel to equip weapons.");
                player.hud.ToggleTutorialTextbox(true);

                break;

            case ShowcaseSection.PEASHOOTER:
                player.hud.SetMissionText("PeaShooter");
                player.hud.SetLevelObjectiveText("Press Y to move on to next section. Press R to reload weapon.");

                player.hud.SetTutorialText("Press or hold the fire button to shoot a stream of bullets.");

                break;

            case ShowcaseSection.RAILGUN:
                player.hud.SetMissionText("Railgun");

                player.hud.SetTutorialText("Hold down fire button to charge a railshot that pierces enemies and applies a burn and slow effect.");

                break;

            case ShowcaseSection.MISSILE:
                player.hud.SetMissionText("Missile");

                player.hud.SetTutorialText("Hold and release fire button to shoot homing rockets at enemies within the visible radius.");

                break;
    
            case ShowcaseSection.PLASMA:
                player.hud.SetMissionText("Revolver");

                player.hud.SetTutorialText("Shoot a plasma bullet that applies a burn effect and slightly damages enemies around its area of effect.");

                break;

            case ShowcaseSection.FLAMETHROWER:
                player.hud.SetMissionText("Flamethrower");

                player.hud.SetTutorialText("Press or hold down the fire button to spit flames that apply a burn effect.");

                break;

            case ShowcaseSection.DASH:
                player.hud.SetMissionText("Dash Ability");
                player.hud.SetLevelObjectiveText("Press Y to move on to next section. Abilities cost resources.");

                player.hud.SetTutorialText("Press LSHIFT to dash in a direction, gain brief invincibility, and push away nearby enemies.");

                break;

            case ShowcaseSection.PHASE:
                player.hud.SetMissionText("Phase Ability");

                player.hud.SetTutorialText("Become intangible and go through all objects.");

                break;

            case ShowcaseSection.SHIELD:
                player.hud.SetMissionText("Shield Ability");

                player.hud.SetTutorialText("Turn on a shield that blocks incoming attacks and projectiles in front of you.");

                break;

            case ShowcaseSection.VORTEX:
                player.hud.SetMissionText("Vortex Ability");

                player.hud.SetTutorialText("Absorb all projectiles. Damage absorbed increases the bonus damage multipler, which applies for 3 seconds.  If too many projectiles are absorbed, the vortex stops and self-damage is inflicted.");

                break;

            case ShowcaseSection.PRACTICE:
                player.hud.SetMissionText("Practice");
                player.hud.SetLevelObjectiveText("Press Y to end tutorial");
                
                player.hud.ToggleTutorialTextbox(false);

                break;
        }
    }

    /*
    - Showcase weapons
    - Showcase abilities
    - Add option to skip tutorial
    */
}
