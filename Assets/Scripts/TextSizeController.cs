using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextSizeController : MonoBehaviour
{
    public Text placeholderText;
    public Text inputText;
    public Text storyText;
    // public variables to adjust font sizes
    public int mediumValue = 24;
    public int largeValue = 30;
    public int xlargeValue = 34;

    private TMP_Dropdown selector;
    private int sizePref;

    void Start() {
        selector = GetComponent<TMP_Dropdown>();

        // Get player size preference, 0 by default if just starting the game.
        sizePref = PlayerPrefs.GetInt("textSize", 0);
        // set the text size on the selector to match the players preference.
        selector.value = sizePref;

        selector.onValueChanged.AddListener(UpdateText);
    }

    void UpdateText(int value) {
        // Update font sizes for all text fields when value is changed.
        PlayerPrefs.SetInt("textSize", value);

        if (value == 0) {
            placeholderText.fontSize = mediumValue;
            inputText.fontSize = mediumValue;
            storyText.fontSize = mediumValue;
        }
        else if (value == 1) {
            placeholderText.fontSize = largeValue;
            inputText.fontSize = largeValue;
            storyText.fontSize = largeValue;
        }
        else if (value == 2) {
            placeholderText.fontSize = xlargeValue;
            inputText.fontSize = xlargeValue;
            storyText.fontSize = xlargeValue;
        }
    }
}
