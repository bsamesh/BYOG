using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{

    public Text doubleJump;
    public Text groundDash;
    public Text airDash;
    public Text shoot;
    public Text shield;
    public Text meelee;
    public Text glide;
    public Text wallSlide;

    public Color enabledColor;
    public Color disabledColor;

    public PlayerMovement player;

    private void OnEnable()
    {
        UpdateAbilities();
        Time.timeScale = 0f;
        UIManager.gamePaused = true;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        UIManager.gamePaused = false;
    }

    private void UpdateAbilities()
    {
        doubleJump.color = player.canDoubleJump ? enabledColor : disabledColor;
        groundDash.color = player.canGroundDash ? enabledColor : disabledColor;
        airDash.color = player.canAirDash ? enabledColor : disabledColor;
        shoot.color = player.canRanged ? enabledColor : disabledColor;
        shield.color = player.canShield ? enabledColor : disabledColor;
        meelee.color = player.canMelee ? enabledColor : disabledColor;
        glide.color = player.canGlide ? enabledColor : disabledColor;
        wallSlide.color = player.canWallslide ? enabledColor : disabledColor;
    }

}