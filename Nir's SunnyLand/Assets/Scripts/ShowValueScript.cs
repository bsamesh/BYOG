using TMPro;
using UnityEngine;

public class ShowValueScript : MonoBehaviour
{
    TextMeshProUGUI percentageText;

    void Awake()
    {
        percentageText = GetComponent<TextMeshProUGUI>();
    }

    public void textUpdate(float value)
    {
        percentageText.text = Mathf.RoundToInt(value * 100) + "%";
    }
}
