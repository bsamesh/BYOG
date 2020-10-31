using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseControl : MonoBehaviour
{
    private bool lostControl = false;
    //[SerializeField] public Player playerMovment;

    //no collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string player = collision.gameObject.name;
        if (!lostControl && player != null)
        {
            Debug.Log("Lost Control!");
            //FindObjectOfType<AudioManager>().Play("Diamond");
            Destroy(gameObject);
            Player.lostControl = true;
            lostControl = true;
            return;
        }
    }
}
