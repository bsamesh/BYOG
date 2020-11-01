using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLevel : MonoBehaviour
{
    bool wasTouched = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string player = collision.gameObject.name;
        if (!wasTouched && player != null)
        {
            Debug.Log("Entered Level!");
            FindObjectOfType<AudioManager>().Play("Crash");
            Destroy(gameObject);
            Player.Crashed();
            Player.lostControl = false;
            Player.hp = Player.maxHp;
            wasTouched = true;
            return;
        }
    }
}
