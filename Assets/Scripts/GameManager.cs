using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	private RoomManager roomManager; 

	void Awake() {
		roomManager = GetComponent<RoomManager> ();
		InitGame ();
	}

	void InitGame() {
		roomManager.generateRoom ();
	}
}
