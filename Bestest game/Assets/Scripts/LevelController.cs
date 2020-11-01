﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static int _nextLevelIndex = 1;

    public void NextLevel()
    {
        _nextLevelIndex++;
        if(_nextLevelIndex < 10)
        {
            Player.hp = Player.maxHp;
            SceneManager.LoadScene("Level_" + _nextLevelIndex);
            //FindObjectOfType<AudioManager>().StopPlayingEverything();
            //FindObjectOfType<AudioManager>().Play("Theme" + _nextLevelIndex);
        }
        else
        {
            return;
            //SceneManager.LoadScene("Victory");
            //FindObjectOfType<AudioManager>().StopPlayingEverything();
            //FindObjectOfType<AudioManager>().Play("Victory");
        }

    }
}
