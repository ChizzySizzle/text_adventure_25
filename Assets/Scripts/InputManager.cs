using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Text storyText; // the story 
    public InputField userInput; // the input field object
    public Text inputText; // part of the input field where user enters response
    public Text placeHolderText; // part of the input field for initial placeholder text
    public Button abutton;
    
    private string story; // holds the story to display
    private List<string> commands = new List<string>(); // Valid user commands

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        commands.Add("go");
        commands.Add("get");

        userInput.onEndEdit.AddListener(GetInput);
        // abutton.onClick.AddListener(DoSomething);
        story = storyText.text;
    }

    void DoSomething() {
        Debug.Log("BUtton Clicked");
    }

    public void UpdateStory(string msg) // Update display
    {
        story += "\n" + msg;
        storyText.text = story;
    }

    void GetInput(string msg) // Process input
    {
        if (msg != "") {
            char[] splitInfo = { ' ' };
            string[] parts = msg.ToLower().Split(splitInfo); //['go', 'north']

            if (commands.Contains(parts[0])){ // if valid input
                if (parts[0] == "go") { // Switch rooms
                    if (NavigationManager.instance.SwitchRooms(parts[1])) {
                        // FILL IN LATER
                    }
                    else {
                        UpdateStory("Exit does not exist. Try again.");
                    }
                }
                else if (parts[0] == "get") { // Add things to inventory
                if (NavigationManager.instance.TakeItem(parts[1])) {
                    GameManager.instance.inventory.Add(parts[1]);
                    UpdateStory($"You've added '{parts[1]}' to your inventory!");
                }
                else {
                    UpdateStory($"Sorry, {parts[1]} does not exist in this room.");
                }
                UpdateStory(msg);
                }
            }
        }

        // reset for next input
        userInput.text = "";
        userInput.ActivateInputField();
    }
}
