using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public static bool gameOver = false;
    public static bool gamePaused = false;

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

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    // Update is called once per frame
    void GameEnded()
    {
        gameOver = true;
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);
    }


    public void RestartLevel()
    {
        Player.hp = Player.maxHp;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOver = false;
        Player.lostControl = true;
    }

}