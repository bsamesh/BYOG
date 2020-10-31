using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    int hp = 100;
    bool isShielded = false;
    float shieldDuration = 0;
    public Image ShieldBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Shield(float time)
    {
        isShielded = true;
        shieldDuration = time;
        //Invoke("Unshield", time);
    }
    public void Unshield()
    {
        isShielded = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (shieldDuration > 0)
        {
            shieldDuration -= Time.deltaTime;
            ShieldBar.fillAmount = shieldDuration;
            if (shieldDuration <= 0)
                Unshield();
        }
    }
}
