using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brain : MonoBehaviour
{
    public string ability = "HELLO";
    void OnEnable()
    {
        foreach (Text text in GetComponentsInChildren<Text>())
        {
            text.text = ability;
        }
    }

    
}
