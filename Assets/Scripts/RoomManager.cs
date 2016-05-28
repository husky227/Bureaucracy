using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour {
	[Serializable]
	public class Count
	{
		public int maximum;
		public int minimum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public const int GRID_WIDTH = 1;
	private const float EPSILON = 0.01f;
	public const int FLOOR_HEIGHT = 5;
	public const float DOOR_WIDTH = 4f;
	public const float WALL_WIDTH = 0.1f;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] ceilingTiles;
	public GameObject[] ceilingLamps;
	public GameObject stamp;
	private Transform roomHolder;

	public Count roomWidth = new Count(5, 15);
	public Count roomLength = new Count(10, 15);

	public GameObject getRandomTile(GameObject[] tiles) {
		int randomFloorTileIndex = Random.Range(0, tiles.Length);
		return tiles[randomFloorTileIndex];
	}

	public void generateRoom(float width, float length, Vector2 position, Vector2 door) {
		Vector3 relativePosition = new Vector3 (position.x, 0, position.y);
		Vector3 centerPosition = new Vector3 (relativePosition.x + width/ 2, 0, relativePosition.z + length/ 2);

		//instantiate floors
		GameObject wall = getRandomTile(wallTiles);
		GameObject floor = getRandomTile(floorTiles);
		GameObject ceiling = getRandomTile(ceilingTiles);
		GameObject lamp = getRandomTile(ceilingLamps);

		//put floor
		Vector3 floorPosition = centerPosition;
		GameObject instance = Instantiate (floor, floorPosition, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
		instance.transform.localScale = new Vector3(width, 0.1f, length);

		Vector3 ceilingPosition = new Vector3 (centerPosition.x, centerPosition.y+5f, centerPosition.z);
		GameObject ceilingInstance = Instantiate (ceiling, ceilingPosition, Quaternion.identity) as GameObject;
		ceilingInstance.transform.SetParent (roomHolder);
		ceilingInstance.transform.localScale = new Vector3(width, 0.1f, length);

		float wallHeight = 5f;
		placeWall (0, width, length, wall, centerPosition, wallHeight, door, true);
		placeWall (1, width, length, wall, centerPosition, wallHeight, door, true);
		placeWall (2, width, length, wall, centerPosition, wallHeight, door, true);
		placeWall (3, width, length, wall, centerPosition, wallHeight, door, true);

		Vector3 lampPosition = new Vector3 (centerPosition.x, centerPosition.y+5f, centerPosition.z);
		GameObject lampInstance = Instantiate (lamp, lampPosition, Quaternion.identity) as GameObject;
		lampInstance.transform.SetParent (roomHolder);
	}

	public void placeWall(int number, float width, float length, GameObject wall, Vector3 centerPosition, float wallHeight, Vector2 door, Boolean checkDoors) {
		Vector3 position = new Vector3 (centerPosition.x, centerPosition.y+wallHeight/2, centerPosition.z);
		Vector3 scale = new Vector3(1f, wallHeight, WALL_WIDTH);
		Vector3 rotate = new Vector3 (0, 0, 0);

		switch (number) {
		case 0:
			position.x -= width / 2 - WALL_WIDTH/2;
			scale.x = length;
			rotate.y = 90;
			break;
		case 1:
			position.z += length / 2 - WALL_WIDTH/2;			
			scale.x = width;
			break;
		case 2:
			position.x += width / 2 - WALL_WIDTH/2;			
			scale.x = length;
			rotate.y = 90;
			break;
		case 3:
			position.z -= length / 2 - WALL_WIDTH/2;
			scale.x = width;
			break;
		}
			
		if (Mathf.Abs (door.y - position.z) < EPSILON && checkDoors && (number == 0 || number == 2)) {
			float startZ = centerPosition.z - length / 2;
			float delta = door.y - startZ;

			Vector3 newPosition1 = new Vector3 (centerPosition.x, centerPosition.y, startZ + (delta - DOOR_WIDTH/2)/2);
			placeWall (number, width, delta - DOOR_WIDTH / 2, wall, newPosition1, wallHeight, door, false);

			Vector3 newPosition2 = new Vector3 (centerPosition.x, centerPosition.y, startZ + length - (length - delta - DOOR_WIDTH/2)/2 );
			placeWall (number, width, length - delta - DOOR_WIDTH / 2, wall, newPosition2, wallHeight, door, false);
			return;
		} else if (Mathf.Abs (door.x - position.x) < EPSILON && checkDoors && (number == 1 || number == 3)) {
			float startX = centerPosition.x - width / 2;
			float delta = door.x - startX;

			Vector3 newPosition1 = new Vector3 (startX + (delta-DOOR_WIDTH/2)/2, centerPosition.y, centerPosition.z);
			placeWall (number, delta - DOOR_WIDTH / 2, length, wall, newPosition1, wallHeight, door, false);

			Vector3 newPosition2 = new Vector3 (startX + width - (width - delta -DOOR_WIDTH/2)/2, centerPosition.y, centerPosition.z);
			placeWall (number, width - delta - DOOR_WIDTH / 2, length, wall, newPosition2, wallHeight, door, false);
			return;
		} else {
			if (!checkDoors) {
				GameObject instance = Instantiate (wall, position, Quaternion.identity) as GameObject;
				instance.transform.SetParent (roomHolder);
				instance.transform.localScale = scale;
				instance.transform.Rotate (rotate);
			}
		}
	}

	public void placeStamp(Vector3 position) {
		GameObject instance = Instantiate (stamp, position, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
		instance.transform.Rotate (0, 0, 0);
	}

	public void generateRandomRoom() {
		int width = Random.Range (roomWidth.minimum, roomWidth.maximum+1);
		int length = Random.Range (roomLength.minimum, roomLength.maximum+1);

		generateRoom(width, length, new Vector2(0, 0), new Vector2(-1000, -1000));
	}

	public void putObjectAtGrid(int indexX, int indexY, int indexZ, Vector3 relativePosition, int angle, GameObject gameObject) {
		Vector3 position = new Vector3 (GRID_WIDTH * indexX + relativePosition.x, GRID_WIDTH * indexY + relativePosition.y, GRID_WIDTH * indexZ + relativePosition.z);
		GameObject instance = Instantiate (gameObject, position, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
		instance.transform.Rotate (0, angle, 0);
	}
}
