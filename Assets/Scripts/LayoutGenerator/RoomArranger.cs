using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomArranger : MonoBehaviour
{
	private int MAX_STAMPS = 3;
	public GameObject[] furniture;
	public GameObject[] corridorGroups;
	public GameObject[] roomGroups;
	public GameObject[] stamps;

	public Transform roomHolder;
	private System.Random random;

	public RoomArranger() {
		int salt = 3835;//38325;
		random = new System.Random (salt);
	}

	public void arrangeFloor(OneCorridorFloorGenerator generator) {
		foreach(Room room in generator.rooms) {
			arrangeRoom (room);
		}
		arrangeCorridor (generator.corridor);
	}

	public void arrangeRoom(Room room) {
		float area = room.size.x*room.size.y;
		if (area <= Config.MAX_BATHROOM_AREA && random.NextDouble() < 0.5) {
			arrangeBathroom (room);
			return;
		}
		if (area <= Config.MAX_STORAGE_AREA && random.NextDouble() < 0.5) {
			arrangeStorageArea (room);
			return;
		}
		placeFurniture (room);
		placeStamps (room);
	}

	public void arrangeCorridor(Corridor corridor) {
		GameObject group = getRandomTile (corridorGroups);
		float length = getLength (group);
		Vector3 scale = new Vector3 (1, 1, 1);
		Vector3 position = new Vector3 (corridor.position.x, corridor.position.y, corridor.position.z);
		Vector3 rotate = new Vector3 (0, 0, 0);
		if (corridor.openSides.east) {
			position = new Vector3 (corridor.position.x+corridor.size.x, corridor.position.y, corridor.position.z+corridor.size.y/2);
			placeObject (group, position, scale, rotate);
		}
		if (corridor.openSides.west) {
			position = new Vector3 (corridor.position.x, corridor.position.y, corridor.position.z+corridor.size.y/2);
			placeObject (group, position, scale, rotate);
		}
		if (corridor.openSides.north) {
			position = new Vector3 (corridor.position.x+corridor.size.x/2, corridor.position.y, corridor.position.z+length/2);
			placeObject (group, position, scale, rotate);
		}
		if (corridor.openSides.south) {
			position = new Vector3 (corridor.position.x+corridor.size.x/2, corridor.position.y, corridor.position.z+corridor.size.y-length/2);
			placeObject (group, position, scale, rotate);
		}
	}

	public void placeFurniture (Room room) {
		float takenArea = 0;
		float roomArea = room.size.x * room.size.y;
		int counter = 0;
		while (takenArea < roomArea*0.4 && counter < 20) {
			counter++; 

			GameObject group = getRandomTile (furniture);
			float width = getWidth (group);
			float length = getLength (group);

			//add some space
			width *= 1.5f;
			length *= 1.5f;
			float minX = width;
			float maxX = room.size.x - width - minX;
			float minY = length;
			float maxY = room.size.y - length - minY;

			float x = (float)random.NextDouble ()*maxX + minX;
			float y = (float)random.NextDouble ()*maxY + minY;

			placeObject(group, new Vector3(room.position.x + x, room.position.y, room.position.z + y), new Vector3(1, 1, 1), new Vector3(0, 0, 0));
		}
	}

	private void arrangeBathroom (Room room) {
		//TODO
	}

	private void arrangeStorageArea(Room room) {
	}

	private void placeStamps(Room room) {
		GameObject stamp = getRandomTile (stamps);
		int noOfStamps = random.Next ()%MAX_STAMPS + 1;
		for (int i = 0; i < noOfStamps; i++) {

			float minX = 0;
			float maxX = room.size.x - minX;
			float minY = 0;
			float maxY = room.size.y - minY;

			float x = (float)random.NextDouble ()*maxX + minX;
			float y = (float)random.NextDouble ()*maxY + minY;
			placeObject(stamp, new Vector3(room.position.x + x, room.position.y, room.position.z + y), new Vector3(1, 1, 1), new Vector3(0, 0, 0));
		}
	}
		
	private void placeObject(GameObject obj, Vector3 position, Vector3 scale, Vector3 rotate) {
		GameObject instance = Instantiate (obj, position, Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder);
		instance.transform.localScale = scale;
		instance.transform.Rotate (rotate);
	}

	private GameObject getRandomTile(GameObject[] tiles) {
		int randomFloorTileIndex = Random.Range(0, tiles.Length);
		return tiles[randomFloorTileIndex];
	}

	private float getWidth(GameObject obj) {
		BoxCollider[] cls = obj.GetComponents<BoxCollider> ();
		BoxCollider[] cls2 = obj.GetComponentsInChildren<BoxCollider> ();
		float width = 0;
		foreach (BoxCollider cl in cls) {
			width = Mathf.Max (width, cl.size.x);
		}
		foreach (BoxCollider cl in cls2) {
			width = Mathf.Max (width, cl.size.x);
		}
		return width;
	}

	private float getLength(GameObject obj) {
		BoxCollider[] cls = obj.GetComponents<BoxCollider> ();
		BoxCollider[] cls2 = obj.GetComponentsInChildren<BoxCollider> ();
		float length = 0;
		foreach (BoxCollider cl in cls) {
			Debug.Log (cl.size.y);
			length = Mathf.Max (length, cl.size.y);
		}
		foreach (BoxCollider cl in cls2) {
			length = Mathf.Max (length, cl.size.y);
		}
		return length;
	}
}

