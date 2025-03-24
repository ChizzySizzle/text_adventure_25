using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public List<string> inventory = new List<string>();

    // dynamic rooms
    private Room orbRoom;
    private Room keyRoom;
    private Room dogRoom;
    private Room dragonRoom;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetRooms(); // initialize dynamic rooms
        
        InputManager.instance.onRestart += ResetGame;
        Load();
    }

    private void GetRooms() { // will loop through the nav managers list of rooms to get the dynamic ones
        foreach (Room room in NavigationManager.instance.rooms) {
            if (room.name == "dragon") {
                dragonRoom = room;
            }
            else if (room.name == "dog") {
                dogRoom = room;
            }
            else if (room.name == "orb") {
                orbRoom = room;
            }
            else if (room.name == "key") {
                keyRoom = room;
            }
        }
    }

    void Load() {
        if (File.Exists(Application.persistentDataPath + "/player.save")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream afile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);
            SaveState playerData = (SaveState) bf.Deserialize(afile);
            afile.Close();

            if (playerData.inventory != null) {
                inventory = playerData.inventory;
            }

            // Check the players loaded inventory and adjust the dynamic room values.
            CheckInventory();

            Room room = NavigationManager.instance.GetRoomFromName(playerData.currentRoom);
            if (room != null) {
                NavigationManager.instance.SwitchRooms(room);
            }
        }
        else {
            NavigationManager.instance.ResetGame();
        }
    }

    public void CheckInventory() { // Checks the players inventory on load, reset, or when items are added to decide the dynamic room states.
        if (inventory.Contains("orb")) {
            orbRoom.hasOrb = false;
            orbRoom.description = "There is an orb shaped hole in this room.";
            NavigationManager.instance.toKeyNorth.isHidden = false;
        }
        else {
            orbRoom.hasOrb = true;
            orbRoom.description = "There is a blue orb in here.";
            NavigationManager.instance.toKeyNorth.isHidden = true;
        }
        if (inventory.Contains("key")) {
            keyRoom.hasKey = false;
            keyRoom.description = "There is some stuff in here.";
        }
        else {
            keyRoom.hasKey = true;
            keyRoom.description = "There is some stuff in here. Is key a key over there?";
        }
        if (inventory.Contains("dog")) {
            dogRoom.hasDog = false;
            dogRoom.description = "There is a dog shaped hole in this room.";
            dragonRoom.description = "There is a dragon in this room!! It blows fire at you, but your trusty dog intercepts it! You then watch in horror as your dog absorbs the dragon. It then absorbs you... You are dead.";
        }
        else {
            dogRoom.hasDog = true;
            dogRoom.description = "There is a cute dog in this room! \"Bark!\". Thats all.";
            dragonRoom.description = "You died. There is dragon in here. Maybe you should have picked the other door.";
        }
    }

    public void Save() {
        SaveState playerState = new SaveState();
        playerState.currentRoom = NavigationManager.instance.currentRoom.name;
        playerState.inventory = inventory;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream afile = File.Create(Application.persistentDataPath + "/player.save");
        Debug.Log(Application.persistentDataPath);

        bf.Serialize(afile, playerState);
        afile.Close();
    }

    void ResetGame() {
        inventory.Clear();
        CheckInventory(); // Update dynamic rooms
    }
}
