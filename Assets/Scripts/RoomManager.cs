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

	public void generateRoom() {
		int width = Random.Range (roomWidth.minimum, roomWidth.maximum+1);
		int length = Random.Range (roomLength.minimum, roomLength.maximum+1);

		//instantiate floors
		for (int i = -width; i <= width; i++) {
			for (int j = -length; j <= length; j++) {
				int randomFloorTileIndex = Random.Range(0, floorTiles.Length);
				GameObject floor = getRandomTile(floorTiles);
				putObjectAtGrid (i, 0, j, 0, floor);

				//instantiate wall
				if (i == -width || i == width || j == -length || j == length) {
					GameObject wall = getRandomTile(wallTiles);
					int angle = 0;
					if (i == -width || i == width) {
						angle = 90;
					}

					putObjectAtGrid (i, 1, j, angle, wall);
				}
			}
		}
	}

	public void putObjectAtGrid(int indexX, int indexY, int indexZ, int angle, GameObject gameObject) {
		Vector3 position = new Vector3 (GRID_WIDTH * indexX, GRID_WIDTH * indexY, GRID_WIDTH * indexZ);
		GameObject instance = Instantiate (gameObject, position, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
		instance.transform.Rotate (0, angle, 0);
	}
}
