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

            Room room = NavigationManager.instance.GetRoomFromName(playerData.currentRoom);
            if (room != null) {
                NavigationManager.instance.SwitchRooms(room);
            }

            if (playerData.inventory != null) {
                inventory = playerData.inventory;
            }
        }
        else {
            NavigationManager.instance.ResetGame();
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
