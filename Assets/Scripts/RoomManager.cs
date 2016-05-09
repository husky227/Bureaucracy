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
	private const float EPSILON = 1.1f;
	public const int FLOOR_HEIGHT = 5;
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

		//instantiate floors
		GameObject wall = getRandomTile(wallTiles);
		GameObject floor = getRandomTile(floorTiles);
		GameObject ceiling = getRandomTile(ceilingTiles);
		GameObject lamp = getRandomTile(ceilingLamps);
		for (int i = 0; i*GRID_WIDTH <= width; i++) {
			for (int j = 0; j*GRID_WIDTH <= length; j++) {
				int randomFloorTileIndex = Random.Range(0, floorTiles.Length);

				putObjectAtGrid (i, 0, j, relativePosition, 0, floor);
				putObjectAtGrid (i, FLOOR_HEIGHT, j, relativePosition, 0, ceiling);

				if (i % 15 == 0 && j % 15 == 0) {
					putObjectAtGrid (i, FLOOR_HEIGHT, j, relativePosition, 0, lamp);
				}

				//instantiate wall
				if (i == 0 || i == width || j == 0 || j == length) {
					Debug.Log (door.x);
					Debug.Log (door.y);
					if (Mathf.Abs(GRID_WIDTH * i + relativePosition.x - door.x) < EPSILON && Mathf.Abs(GRID_WIDTH * j + relativePosition.z - door.y) < EPSILON ) {
						continue;
					}

					int angle = 0;
					if (i == 0 || i == width) {
						angle = 90;
					}

					putObjectAtGrid (i, 1, j, relativePosition, angle, wall);
				}
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
