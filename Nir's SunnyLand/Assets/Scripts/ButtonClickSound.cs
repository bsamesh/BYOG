using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public void PlayClickSound()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }
}
