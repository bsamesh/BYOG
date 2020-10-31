using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevel : MonoBehaviour
{
    bool wasTouched = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (!wasTouched && player != null)
        {
            wasTouched = true;
            Debug.Log("Exit Level!");
            FindObjectOfType<LevelController>().NextLevel();
            Player.lostControl = false;
            return;
        }
    }
}
