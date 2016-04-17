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
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	private Transform roomHolder;

	public Count roomWidth = new Count(5, 15);
	public Count roomLength = new Count(10, 15);

	public GameObject getRandomTile(GameObject[] tiles) {
		int randomFloorTileIndex = Random.Range(0, tiles.Length);
		return tiles[randomFloorTileIndex];
	}

	public void generateRoom(float width, float length, Vector2 position) {
		print (width);
		print (length);
		print (position.x);
		print (position.y);
		Vector3 relativePosition = new Vector3 (position.x, 0, position.y);
		//instantiate floors
		for (int i = 0; i*GRID_WIDTH <= width; i++) {
			for (int j = 0; j*GRID_WIDTH <= length; j++) {
				int randomFloorTileIndex = Random.Range(0, floorTiles.Length);
				GameObject floor = getRandomTile(floorTiles);
				putObjectAtGrid (i, 0, j, relativePosition, 0, floor);

				//instantiate wall
				if (i == 0 || i == width || j == 0 || j == length) {
					GameObject wall = getRandomTile(wallTiles);
					int angle = 0;
					if (i == 0 || i == width) {
						angle = 90;
					}

					putObjectAtGrid (i, 1, j, relativePosition, angle, wall);
				}
			}
		}
	}

	public void generateRandomRoom() {
		int width = Random.Range (roomWidth.minimum, roomWidth.maximum+1);
		int length = Random.Range (roomLength.minimum, roomLength.maximum+1);

		generateRoom(width, length, new Vector2(0, 0));
	}

	public void putObjectAtGrid(int indexX, int indexY, int indexZ, Vector3 relativePosition, int angle, GameObject gameObject) {
		Vector3 position = new Vector3 (GRID_WIDTH * indexX + relativePosition.x, GRID_WIDTH * indexY + relativePosition.y, GRID_WIDTH * indexZ + relativePosition.z);
		GameObject instance = Instantiate (gameObject, position, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
		instance.transform.Rotate (0, angle, 0);
	}
}
