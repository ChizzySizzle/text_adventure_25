using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public List<string> inventory = new List<string>();

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
        InputManager.instance.onRestart += ResetGame;
        Load();
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

    private void CheckInventory() {
        foreach (string item in inventory) {
            if (inventory.Contains("orb")) {
                NavigationManager.instance.orbRoom.hasOrb = false;
                NavigationManager.instance.orbRoom.description = "There is an orb shaped hole in this room.";
            }
            if (inventory.Contains("key")) {
                NavigationManager.instance.keyRoom.hasKey = false;
                NavigationManager.instance.keyRoom.description = "There is some stuff in here.";
            }
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
    }
}
