using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider slider;
    private float maxHP;

    private void Start()
    {
        Player.PlayerTookDamage += SetBar;
        maxHP = Player.hp;
        slider = GetComponentInChildren<Slider>();
    }

    void SetBar()
    {
        slider.value = Player.hp / maxHP;
    }
}
