using UnityEngine;
using System.Collections;

public class FloorRenderer : MonoBehaviour
{
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] ceilingTiles;
	public GameObject[] ceilingLamps;
	public GameObject[] doors;
	public GameObject[] elevators;

	public Transform roomHolder;

	public void renderFloor(OneCorridorFloorGenerator generator) {
		GameObject floor = getRandomTile(floorTiles);
		GameObject door = getRandomTile(doors);
		GameObject ceiling = getRandomTile(ceilingTiles);
		GameObject lamp = getRandomTile(ceilingLamps);
		GameObject elevator = getRandomTile(elevators);

		drawFloor (generator.corridor.position, generator.corridor.size, floor);
		drawElevator (generator.corridor.position, generator.corridor.size, elevator);
		drawCeiling (generator.corridor.position, generator.corridor.size, ceiling);
		floor = getRandomTile(floorTiles);
		GameObject wall = getRandomTile(wallTiles);
		foreach(Room room in generator.rooms) {
			drawFloor (room.position, room.size, floor);
			drawCeiling (room.position, room.size, ceiling);
			drawWalls (room, wall);
			placeDoors (room, door);
			placeLamps (room.position, room.size, lamp);
		}

		placeLamps (generator.corridor.position, generator.corridor.size, lamp);
		drawCorridorFloors (generator.corridor, wall);
	}

	public void drawElevator (Vector3 position, Vector2 size, GameObject elevator) {
		Vector3 elevatorPosition = new Vector3(position.x + size.x/2, position.y, position.z + size.y/2);
		GameObject instance = Instantiate (elevator, elevatorPosition, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
	}

	public void drawFloor(Vector3 position, Vector2 size, GameObject floor) {
		Vector3 floorPosition = new Vector3(position.x + size.x/2, position.y-0.1f, position.z + size.y/2);
		GameObject instance = Instantiate (floor, floorPosition, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
		instance.transform.localScale = new Vector3(size.x, 0.1f, size.y);
	}

	public void drawCeiling(Vector3 position, Vector2 size, GameObject ceiling) {
		Vector3 ceilingPosition = new Vector3(position.x + size.x/2, position.y + Config.FLOOR_HEIGHT-0.5f, position.z + size.y/2);
		GameObject instance = Instantiate (ceiling, ceilingPosition, Quaternion.identity) as GameObject;
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

	private void placeDoors (Room room, GameObject doors) {
		if (room.doorSide == Side.WEST) {
			Vector3 position = new Vector3(room.position.x, 0, room.position.z+ room.doorPosition - Config.DOOR_WIDTH/4 );
			GameObject instance = Instantiate (doors, position, Quaternion.identity) as GameObject;
			instance.transform.SetParent (roomHolder);
			instance.transform.Rotate(new Vector3 (0, 90, 0));
		}
		if (room.doorSide == Side.EAST) {
			Vector3 position = new Vector3(room.position.x  + room.size.x, 0, room.position.z+ room.doorPosition - Config.DOOR_WIDTH/4 );
			GameObject instance = Instantiate (doors, position, Quaternion.identity) as GameObject;
			instance.transform.SetParent (roomHolder);
			instance.transform.Rotate(new Vector3 (0, 90, 0));
		}
		if (room.doorSide == Side.NORTH) {
			Vector3 position = new Vector3(room.position.x  + room.doorPosition - Config.DOOR_WIDTH/4 , 0, room.position.z);
			GameObject instance = Instantiate (doors, position, Quaternion.identity) as GameObject;
			instance.transform.SetParent (roomHolder);
		}
		if (room.doorSide == Side.SOUTH) {
			Vector3 position = new Vector3(room.position.x  + room.doorPosition - Config.DOOR_WIDTH/4 , 0, room.position.z + room.size.z);
			GameObject instance = Instantiate (doors, position, Quaternion.identity) as GameObject;
			instance.transform.SetParent (roomHolder);
		}
	}

	private void drawWall(GameObject wall, Vector3 position, Vector3 scale) {
		GameObject instance = Instantiate (wall, position, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
		instance.transform.localScale = scale;
	}

	private void placeLamps(Vector3 position, Vector2 size, GameObject lamp) {
		float width = size.x;
		float length = size.y;
		float lampHeight = lamp.GetComponent<BoxCollider> ().size.y/2;

		for (float x = (width % Config.LAMP_AREA) / 2; x <= width; x += Config.LAMP_AREA) {
			for (float y = (length %  Config.LAMP_AREA) / 2; y <= length; y += Config.LAMP_AREA) {
				Vector3 lampPosition = new Vector3 (position.x + x, Config.FLOOR_HEIGHT+position.y-lampHeight, position.z + y);
				GameObject lampInstance = Instantiate (lamp, lampPosition, Quaternion.identity) as GameObject;
				lampInstance.transform.SetParent (roomHolder);
			}
		}
	}
		
	private GameObject getRandomTile(GameObject[] tiles) {
		int randomFloorTileIndex = Random.Range(0, tiles.Length);
		return tiles[randomFloorTileIndex];
	}

	private void drawCorridorFloors(Corridor corridor, GameObject wall) {
		if (corridor.openSides.east) {
			Vector3 wallPosition = new Vector3(corridor.position.x + corridor.size.x, corridor.position.y+Config.FLOOR_HEIGHT/2, corridor.position.z + corridor.size.y/2);
			Vector3 scale = new Vector3(0.1f, Config.FLOOR_HEIGHT, corridor.size.y);
			drawWall (wall, wallPosition, scale);
		}
		if (corridor.openSides.west) {
			Vector3 wallPosition = new Vector3(corridor.position.x, corridor.position.y+Config.FLOOR_HEIGHT/2, corridor.position.z + corridor.size.y/2);
			Vector3 scale = new Vector3(0.1f, Config.FLOOR_HEIGHT, corridor.size.y);
			drawWall (wall, wallPosition, scale);
		}
		if (corridor.openSides.north) {
			Vector3 wallPosition = new Vector3(corridor.position.x + corridor.size.x/2, corridor.position.y+Config.FLOOR_HEIGHT/2, corridor.position.z);
			Vector3 scale = new Vector3(corridor.size.x, Config.FLOOR_HEIGHT, 0.1f);
			drawWall (wall, wallPosition, scale);
		}
		if (corridor.openSides.south) {
			Vector3 wallPosition = new Vector3(corridor.position.x + corridor.size.x/2, corridor.position.y+Config.FLOOR_HEIGHT/2, corridor.position.z - corridor.size.y);
			Vector3 scale = new Vector3(corridor.size.x, Config.FLOOR_HEIGHT, 0.1f);
			drawWall (wall, wallPosition, scale);
		}
	}
}

