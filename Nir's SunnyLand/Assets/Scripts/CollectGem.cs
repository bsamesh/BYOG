using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGem : MonoBehaviour
{
    [SerializeField] UpdatePanel panel;
    private bool gotCollected = false;

    //no collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string player = collision.gameObject.name;
        if (!gotCollected && player != null)
        {
            Debug.Log("Gem collected!");
            FindObjectOfType<AudioManager>().Play("Diamond");
            Destroy(gameObject);
            panel.UpdateGems();
            gotCollected = true;
            return;
        }

    }

    //collision version
    /* private void OnCollisionEnter2D(Collision2D collision)
    {

        Player player = collision.collider.GetComponent<Player>();
        if (!gotCollected && player != null)
        {
            Debug.Log("Gem collected!");
            FindObjectOfType<AudioManager>().Play("Diamond");
            Destroy(gameObject);
            panel.UpdateGems();
            gotCollected = true;
            return;
        }
    }*/
}
