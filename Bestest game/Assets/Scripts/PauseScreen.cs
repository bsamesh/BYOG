using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{

    private void OnEnable()
    {
        Time.timeScale = 0f;
        UIManager.gamePaused = true;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        UIManager.gamePaused = false;
    }

}