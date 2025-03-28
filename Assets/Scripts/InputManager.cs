using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
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
    public Scrollbar scrollbar;

    // public Button abutton;
    public delegate void Restart();
    public event Restart onRestart;
    
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
        commands.Add("restart");
        commands.Add("save");
        commands.Add("inventory");
        commands.Add("commands");

        userInput.onEndEdit.AddListener(GetInput);
        
        // abutton.onClick.AddListener(DoSomething);

        story = storyText.text;

        NavigationManager.instance.onGameOver += EndGame;
        onRestart += StarterText;
    }

    public void StarterText() { // helpful starter text, will clear previous story on restart.
        story = "";
        UpdateStory("Enter the word 'commands' for a full list of commands!");
    }
    
    void EndGame() {
        UpdateStory("\nPlease enter 'restart' to play again");
    }

    // void DoSomething() {
    //     Debug.Log("BUtton Clicked");
    // }

    public void UpdateStory(string msg) // Update display
    {
        story += "\n" + msg + "\n";
        storyText.text = story;
    }

    public void GetInput(string msg) // Process input
    {
        if (msg != "") {
            char[] splitInfo = { ' ' };
            string[] parts = msg.ToLower().Split(splitInfo); //['go', 'north']

            if (commands.Contains(parts[0])){ // if valid input
                UpdateStory(msg);
                if (parts[0] == "go") { // Switch rooms
                    if (NavigationManager.instance.SwitchRooms(parts[1])) {
                        // FILL IN LATER
                    }
                    else {
                        UpdateStory("Exit does not exist or is locked. Try again.");
                    }
                }
                else if (parts[0] == "get") { // Add things to inventory
                    if (NavigationManager.instance.TakeItem(parts[1])) {
                        GameManager.instance.inventory.Add(parts[1]);
                        // Check inventory to refresh inventory and adjust dynamic room values.
                        GameManager.instance.CheckInventory();
                        UpdateStory($"You've added '{parts[1]}' to your inventory!");
                    }
                    else {
                        UpdateStory($"'{parts[1]}' does not exist or has been picked up.");
                    }
                }
                else if (parts[0] == "restart") {
                    if(onRestart != null)
                        onRestart();
                }
                else if (parts[0] == "save") {
                    GameManager.instance.Save();
                    UpdateStory("Save Successful");
                }
                // Added inventory commands that loops through the inventory list if it contains items
                else if (parts[0] == "inventory") {
                    UpdateStory("Your inventory currently contains: ");
                    // inventory is empty
                    if (GameManager.instance.inventory.Count == 0) {
                        UpdateStory("Nothing!");
                    }
                    // inventory is not empty
                    else { 
                        foreach (string item in GameManager.instance.inventory) {
                            UpdateStory($"1x '{item}'");
                        }
                    }
                }
                // Added commands command to display possible commands
                else if (parts[0] == "commands") {
                    UpdateStory("go (north, south, east, west)\nget (object)\nrestart\nsave\ninventory\ncommands/help");
                }
            }
            else {
                // response to gibberish
                UpdateStory("Command does not exist, try again.");
            }
        }

        // reset for next input
        userInput.text = "";
        userInput.ActivateInputField();
    }
}
