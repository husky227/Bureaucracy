using UnityEngine;
using System.Collections;

public class FloorRenderer : MonoBehaviour
{
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] ceilingTiles;
	public GameObject[] ceilingLamps;

	public Transform roomHolder;

	public void renderFloor(OneCorridorFloorGenerator generator) {
		GameObject floor = getRandomTile(floorTiles);
		drawFloor (generator.corridor.position, generator.corridor.size, floor);
		floor = getRandomTile(floorTiles);
		GameObject wall = getRandomTile(wallTiles);
		foreach(Room room in generator.rooms) {
			drawFloor (room.position, room.size, floor);
			drawWalls (room, wall);
		}

		drawCorridorFloors (generator.corridor, wall);
	}

	public void drawFloor(Vector3 position, Vector2 size, GameObject floor) {
		Vector3 floorPosition = new Vector3(position.x + size.x/2, position.y, position.z + size.y/2);
		GameObject instance = Instantiate (floor, floorPosition, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
		instance.transform.localScale = new Vector3(size.x, 0.1f, size.y);
	}

	public void drawWalls(Room room, GameObject wall) {
		if (room.doorSide != Side.WEST) {
			Vector3 wallPosition = new Vector3(room.position.x, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z + room.size.y/2);
			Vector3 scale = new Vector3(0.1f, Config.FLOOR_HEIGHT, room.size.y);
			drawWall (wall, wallPosition, scale);
		} else {
			float sizeLeft = (room.doorPosition - Config.DOOR_WIDTH / 2);
			float positionLeft = (room.doorPosition - Config.DOOR_WIDTH / 2)/2;
			Vector3 wallPosition1 = new Vector3(room.position.x, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z + positionLeft );
			Vector3 scale1 = new Vector3(0.1f, Config.FLOOR_HEIGHT, sizeLeft);
			drawWall (wall, wallPosition1, scale1);

			float sizeRight = (room.size.y - sizeLeft - Config.DOOR_WIDTH/2);
			float positionRight = room.size.y - sizeRight/2;
			Vector3 wallPosition2 = new Vector3(room.position.x, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z + positionRight );
			Vector3 scale2 = new Vector3(0.1f, Config.FLOOR_HEIGHT, sizeRight);
			drawWall (wall, wallPosition2, scale2);
		}
		if (room.doorSide != Side.EAST) {
			Vector3 wallPosition = new Vector3(room.position.x + room.size.x, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z + room.size.y/2);
			Vector3 scale = new Vector3(0.1f, Config.FLOOR_HEIGHT, room.size.y);
			drawWall (wall, wallPosition, scale);
		} else {
			float sizeLeft = (room.doorPosition - Config.DOOR_WIDTH / 2);
			float positionLeft = (room.doorPosition - Config.DOOR_WIDTH / 2)/2;
			Vector3 wallPosition1 = new Vector3(room.position.x + room.size.x, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z + positionLeft );
			Vector3 scale1 = new Vector3(0.1f, Config.FLOOR_HEIGHT, sizeLeft);
			drawWall (wall, wallPosition1, scale1);

			float sizeRight = (room.size.y - sizeLeft - Config.DOOR_WIDTH/2);
			float positionRight = room.size.y - sizeRight/2;
			Vector3 wallPosition2 = new Vector3(room.position.x + room.size.x, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z + positionRight );
			Vector3 scale2 = new Vector3(0.1f, Config.FLOOR_HEIGHT, sizeRight);
			drawWall (wall, wallPosition2, scale2);
		}
		if (room.doorSide != Side.NORTH) {
			Vector3 wallPosition = new Vector3(room.position.x+ room.size.x/2, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z );
			Vector3 scale = new Vector3(room.size.x, Config.FLOOR_HEIGHT, 0.1f);
			drawWall (wall, wallPosition, scale);
		} else {
			float sizeLeft = (room.doorPosition - Config.DOOR_WIDTH / 2);
			float positionLeft = (room.doorPosition - Config.DOOR_WIDTH / 2)/2;
			Vector3 wallPosition1 = new Vector3(room.position.x + positionLeft, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z );
			Vector3 scale1 = new Vector3(sizeLeft, Config.FLOOR_HEIGHT, 0.1f);
			drawWall (wall, wallPosition1, scale1);

			float sizeRight = (room.size.x - sizeLeft - Config.DOOR_WIDTH/2);
			float positionRight = room.size.x - sizeRight/2;
			Vector3 wallPosition2 = new Vector3(room.position.x + positionRight, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z );
			Vector3 scale2 = new Vector3(sizeRight, Config.FLOOR_HEIGHT, 0.1f);
			drawWall (wall, wallPosition2, scale2);
		}
		if (room.doorSide != Side.SOUTH) {
			Vector3 wallPosition = new Vector3(room.position.x + room.size.x/2, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z + room.size.y );
			Vector3 scale = new Vector3(room.size.x, Config.FLOOR_HEIGHT, 0.1f);
			drawWall (wall, wallPosition, scale);
		} else {
			float sizeLeft = (room.doorPosition - Config.DOOR_WIDTH / 2);
			float positionLeft = (room.doorPosition - Config.DOOR_WIDTH / 2)/2;
			Vector3 wallPosition1 = new Vector3(room.position.x + positionLeft, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z + room.size.y );
			Vector3 scale1 = new Vector3(sizeLeft, Config.FLOOR_HEIGHT, 0.1f);
			drawWall (wall, wallPosition1, scale1);

			float sizeRight = (room.size.x - sizeLeft - Config.DOOR_WIDTH/2);
			float positionRight = room.size.x - sizeRight/2;
			Vector3 wallPosition2 = new Vector3(room.position.x + positionRight, room.position.y+Config.FLOOR_HEIGHT/2, room.position.z + room.size.y  );
			Vector3 scale2 = new Vector3(sizeRight, Config.FLOOR_HEIGHT, 0.1f);
			drawWall (wall, wallPosition2, scale2);
		}
	}

	private void drawWall(GameObject wall, Vector3 position, Vector3 scale) {
		GameObject instance = Instantiate (wall, position, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
		instance.transform.localScale = scale;
	}
		
	private GameObject getRandomTile(GameObject[] tiles) {
		int randomFloorTileIndex = Random.Range(0, tiles.Length);
		return tiles[randomFloorTileIndex];
	}

	private void drawCorridorFloors(Corridor corridor, GameObject wall) {
		if (corridor.openSides.east) {
			Debug.Log ("east");
			Vector3 wallPosition = new Vector3(corridor.position.x + corridor.size.x, corridor.position.y+Config.FLOOR_HEIGHT/2, corridor.position.z + corridor.size.y/2);
			Vector3 scale = new Vector3(0.1f, Config.FLOOR_HEIGHT, corridor.size.y);
			drawWall (wall, wallPosition, scale);
		}
		if (corridor.openSides.west) {
			Debug.Log ("west");
			Vector3 wallPosition = new Vector3(corridor.position.x, corridor.position.y+Config.FLOOR_HEIGHT/2, corridor.position.z + corridor.size.y/2);
			Vector3 scale = new Vector3(0.1f, Config.FLOOR_HEIGHT, corridor.size.y);
			drawWall (wall, wallPosition, scale);
		}
		if (corridor.openSides.north) {
			Debug.Log ("north");
			Vector3 wallPosition = new Vector3(corridor.position.x + corridor.size.x/2, corridor.position.y+Config.FLOOR_HEIGHT/2, corridor.position.z);
			Vector3 scale = new Vector3(corridor.size.x, Config.FLOOR_HEIGHT, 0.1f);
			drawWall (wall, wallPosition, scale);
		}
		if (corridor.openSides.south) {
			Debug.Log ("south");
			Vector3 wallPosition = new Vector3(corridor.position.x + corridor.size.x/2, corridor.position.y+Config.FLOOR_HEIGHT/2, corridor.position.z - corridor.size.y);
			Vector3 scale = new Vector3(corridor.size.x, Config.FLOOR_HEIGHT, 0.1f);
			drawWall (wall, wallPosition, scale);
		}
	}
}

