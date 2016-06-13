using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	private MiniMapGenerator minimapGenerator;
	/*private RoomManager roomManager; 
	private FloorGenerator floorGenerator; 
	*/

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

		minimapGenerator = GetComponent<MiniMapGenerator> ();

		/*roomManager = GetComponent<RoomManager> ();
		floorGenerator = GetComponent<FloorGenerator> ();
		floorGenerator.setRoomManager (roomManager);*/
		InitGame ();
	}

	void InitGame() {
		/*floorGenerator.generateFloor ();*/
		minimapGenerator.generateMiniMap (floorGenerator.rooms);
	}
}
