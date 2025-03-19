using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public Image background;
    public Text displayText;
    public Text toggleText;
    public Text placeholderText;
    public Text playerText;
        
    private bool darkMode;
    
    // Start is called before the first frame update
    void Start()
    {
        Toggle toggle = GetComponent<Toggle>();
        darkMode = toggle.isOn;
        
        int themePref = PlayerPrefs.GetInt("theme", 1);

        if (themePref == 1) {
            toggle.isOn = true;
            darkMode = true;
        }
        else {
            toggle.isOn = false;
            darkMode = false;
        }

        SetTheme();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool value) {
        darkMode = value;
        PlayerPrefs.SetInt("theme", darkMode ? 1 : 0);
        SetTheme();
    }

    void SetTheme() {
        if (darkMode) {
            background.color = Color.black;
            displayText.color = Color.white;
            toggleText.color = Color.white;
            placeholderText.color = Color.white;
            playerText.color = Color.white;
        }
        else {
            background.color = Color.white;
            displayText.color = Color.black;
            toggleText.color = Color.black;
            placeholderText.color = Color.black;
            playerText.color = Color.black;
        }
    }
}
