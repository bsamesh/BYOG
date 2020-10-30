using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatePanel : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI gemsText;
    [SerializeField] TextMeshProUGUI cherryText;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] GameObject levelExit;
    private int gems = 0;
    private int cherrys = 0;

    // Update is called once per frame
    public void UpdateGems()
    {
        gems++;
        gemsText.SetText(": " + gems + "/10");
        if (gems == 10 && cherrys == 10)
            UpdateMessage();
    }

    public void UpdateCherrys()
    {
        cherrys++;
        cherryText.SetText(": " + cherrys + "/10");
        if (gems == 10 && cherrys == 10)
            UpdateMessage();
    }

    private void UpdateMessage()
    {
        FindObjectOfType<AudioManager>().Play("Message");
        messageText.SetText("GO BACK TO THE HOUSE!");
        levelExit.SetActive(true);
    }

}
