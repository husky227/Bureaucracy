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
	public GameObject[] npcs;

	public Transform roomHolder;
	private System.Random random;

	class Rectangle {
		public Vector2 position;
		public Vector2 size;

		public bool IsIntersectingX(Rectangle room) {
			if (room.position.x < position.x) {
				return room.position.x + room.size.x >= position.x;
			}
			return position.x + size.x >= room.position.x;
		}

		public bool IsIntersectingY(Rectangle room) {
			if (room.position.y < position.y) {
				return room.position.y + room.size.y >= position.y;
			}
			return position.y + size.y >= room.position.y;
		}

		public bool IsIntersecting(Rectangle room) {
			return IsIntersectingX(room) && IsIntersectingY(room);
		}
	}

	private List<Rectangle> furnitures = new List<Rectangle>();

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
		furnitures = new List<Rectangle> ();

		bool isNpcSet = false;
		bool isNpcNeeded = (random.NextDouble () > 0.5f);
		bool settingNpc  = false;

		while (takenArea < roomArea*0.4 && counter < 200) {
			counter++; 

			GameObject group = getRandomTile (furniture);

			if (!isNpcSet && isNpcNeeded) {
				group = getRandomTile (npcs);
				settingNpc = true;
			}

			float width = getWidth (group);
			float length = getLength (group);

			//add some space
			width = width + 2f;
			length = length + 2f;
			float minX = width;
			float maxX = room.size.x - width - minX;
			float minY = length;
			float maxY = room.size.y - length - minY;

			float x = (float)random.NextDouble ()*maxX + minX;
			float y = (float)random.NextDouble ()*maxY + minY;

			Rectangle furnitureRectangle = new Rectangle ();
			furnitureRectangle.position = new Vector2 (x, y);
			furnitureRectangle.size = new Vector2 (width, length);
			bool intersect = IsIntersecting (furnitureRectangle);

			if (!intersect) {
				furnitures.Add (furnitureRectangle);
				placeObject (group, new Vector3 (room.position.x + x, room.position.y, room.position.z + y), new Vector3 (1, 1, 1), new Vector3 (0, 0, 0));

				if (settingNpc) {
					settingNpc = false;
					isNpcNeeded = false;
					isNpcSet = true;
				}
			}
		}
	}

	private bool IsIntersecting(Rectangle furniture) {
		bool isIntersecting = false;
		foreach(Rectangle rectangle in furnitures) {
			isIntersecting = isIntersecting || rectangle.IsIntersecting (furniture);
		}
		return isIntersecting;
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
		float minLength = 0;
		float maxLength = 0;
		foreach (BoxCollider cl in cls) {
			minLength = Mathf.Min (minLength, cl.bounds.min.x);
			maxLength = Mathf.Max (maxLength, cl.bounds.max.x);
		}
		foreach (BoxCollider cl in cls2) {
			minLength = Mathf.Min (minLength, cl.bounds.min.x);
			maxLength = Mathf.Max (maxLength, cl.bounds.max.x);
		}
		return maxLength-minLength;
	}

	private float getLength(GameObject obj) {
		BoxCollider[] cls = obj.GetComponents<BoxCollider> ();
		BoxCollider[] cls2 = obj.GetComponentsInChildren<BoxCollider> ();
		float minLength = 0;
		float maxLength = 0;
		foreach (BoxCollider cl in cls) {
			minLength = Mathf.Min (minLength, cl.bounds.min.z);
			maxLength = Mathf.Max (maxLength, cl.bounds.max.z);
		}
		foreach (BoxCollider cl in cls2) {
			minLength = Mathf.Min (minLength, cl.bounds.min.z);
			maxLength = Mathf.Max (maxLength, cl.bounds.max.z);
		}
		return maxLength-minLength;
	}
}

