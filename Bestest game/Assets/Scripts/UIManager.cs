using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{

    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    private bool gameOver = false;

    public static Action pausePressed;

    void Start()
    {
        Player.PlayerDied += GameEnded;
    }

    void Update()
    {
        if (!gameOver && Input.GetKeyDown(KeyCode.P))
        {
            pauseScreen.SetActive(!pauseScreen.activeSelf);
        }
    }

    // Update is called once per frame
    void GameEnded()
    {
        gameOver = true;
        gameOverScreen.SetActive(true);
    }
}
