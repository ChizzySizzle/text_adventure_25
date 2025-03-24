using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button butto;

    // Start is called before the first frame update
    void Start()
    {
        butto = gameObject.GetComponent<Button>();

         // when butto clicked
        butto.onClick.AddListener(Help);
    }

    void Help() { // display the output for the commands command
        InputManager.instance.GetInput("commands");
    }
}
