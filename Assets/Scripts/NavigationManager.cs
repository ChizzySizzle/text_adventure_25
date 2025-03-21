using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance;
    public Room startingRoom;
    public Room currentRoom;
    public List<Room> rooms;

    public delegate void GameOver();
    public event GameOver onGameOver;

    public Exit toKeyNorth;

    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();
    public Room orbRoom;
    public Room keyRoom;

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


        // toKeyNorth.isHidden = true;
        // currentRoom = startingRoom;
        // Unpack();
    }

    public void ResetGame() {
        toKeyNorth.isHidden = true;
        currentRoom = startingRoom;
        orbRoom.hasOrb = true;
        orbRoom.description = "There is a blue orb in here.";
        keyRoom.hasKey = true;
        keyRoom.description = "There is some stuff in here. Is key a key over there?";
        Unpack();
    }

    void Unpack() {
        if (currentRoom == orbRoom && GameManager.instance.inventory.Contains("orb")) {
            orbRoom.description = "There is an orb shaped hole in this room.";
        }
        if (currentRoom == keyRoom && GameManager.instance.inventory.Contains("key")) {
            keyRoom.description = "There is some stuff in here.";
        }
        string description = currentRoom.description;
        exitRooms.Clear();
        foreach (Exit e in currentRoom.exits) {
            if (!e.isHidden) {
                description += ' ' + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            }
        }

        InputManager.instance.UpdateStory(description);

        if (exitRooms.Count == 0) {
            if (onGameOver != null) {
                onGameOver();
            }
        }
    }

    public bool SwitchRooms(string dir) {
        if (exitRooms.ContainsKey(dir)) {
            if (!GetExit(dir).isLocked || GameManager.instance.inventory.Contains("key")) {
                InputManager.instance.UpdateStory("You go " + dir);
                currentRoom = exitRooms[dir];
                Unpack();
                return true;
            }
        }

        return false;
    }

    public void SwitchRooms(Room room) {
        currentRoom = room;
        Unpack();
    }

    Exit GetExit(string dir) {
        foreach(Exit e in currentRoom.exits) {
            if (e.direction.ToString() == dir) {
                return e;
            }
        }
        return null;
    }

    public bool TakeItem(string item) {
        if (item == "key" && currentRoom.hasKey) {
            currentRoom.hasKey = false;
            return true;
        }
        else if (item == "orb" && currentRoom.hasOrb) {
            currentRoom.hasOrb = false;
            toKeyNorth.isHidden = false;
            return true;
        }
        else
            return false;
    }

    public Room GetRoomFromName(string name) {
        foreach (Room aRoom in rooms) {
            if (aRoom.name == name) {
                return aRoom;
            }
        }
        return null;
    }
}
