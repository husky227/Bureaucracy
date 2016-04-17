using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	private RoomManager roomManager; 
	private FloorGenerator floorGenerator; 

	void Awake() {
		roomManager = GetComponent<RoomManager> ();
		floorGenerator = GetComponent<FloorGenerator> ();
		floorGenerator.setRoomManager (roomManager);
		InitGame ();
	}

	void InitGame() {
		floorGenerator.generateFloor ();
	}
}
