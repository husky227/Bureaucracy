using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	/*private RoomManager roomManager; 
	private FloorGenerator floorGenerator; 
	private MiniMapGenerator minimapGenerator;*/

	private OneCorridorFloorGenerator floorGenerator;
	private FloorRenderer renderer;
	private RoomArranger arranger;

	void Awake() {
		floorGenerator = GetComponent<OneCorridorFloorGenerator> ();
		floorGenerator.generateFloor ();

		renderer = GetComponent<FloorRenderer> ();
		arranger = GetComponent<RoomArranger> ();
		renderer.renderFloor (floorGenerator);
		arranger.arrangeFloor (floorGenerator);

		/*roomManager = GetComponent<RoomManager> ();
		floorGenerator = GetComponent<FloorGenerator> ();
		minimapGenerator = GetComponent<MiniMapGenerator> ();
		floorGenerator.setRoomManager (roomManager);*/
		InitGame ();
	}

	void InitGame() {
		/*floorGenerator.generateFloor ();
		minimapGenerator.generateMiniMap (floorGenerator.rooms);*/
	}
}
