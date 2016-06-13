using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorGenerator : MonoBehaviour {
	private const float SPLIT_RANDOM_RANGE_MIN = 0.35f;
	private const float SPLIT_RANDOM_RANGE_MAX = 0.65f;

	private const int CORRIDOR_WIDTH = 4;

	public List<RoomInfo> rooms = new List <RoomInfo> ();

	public class Node {
		public Node left = null;
		public Node center = null;
		public Node right = null;

		public Vector2 start;
		public Vector2 end;

		public Vector2 doors; //if has doors

		public Node(Vector2 start, Vector2 end, Vector2 doors) {
			this.start = start;
			this.end = end;
			this.doors = doors;
		}

		public void split(bool rotated, bool corridor, bool hasDoors) {
			float width = Mathf.Abs (start.x - end.x);
			float length = Mathf.Abs (start.y - end.y);

			float splitted = Random.Range(SPLIT_RANDOM_RANGE_MIN*100, SPLIT_RANDOM_RANGE_MAX*100)/100.0f;
			float margin = (corridor) ? CORRIDOR_WIDTH / 2 : 0;

			if (rotated) {
				//split width
				float newX = Mathf.Floor(start.x + width*splitted);
				Vector2 doors = (hasDoors)? new Vector2(newX-margin, start.y+length/2) : new Vector2(-1000, -1000);
				left = new Node(new Vector2(start.x, start.y), new Vector2(newX-margin, end.y), doors);
				center = (corridor)? new Node(new Vector2(newX-margin, start.y), new Vector2(newX+margin, end.y), doors) : null;
				right = new Node(new Vector2(newX+margin, start.y), new Vector2(end.x, end.y), doors);
			} else {
				//split length
				float newY = Mathf.Floor(start.y + length*splitted);
				Vector2 doors = (hasDoors)? new Vector2(start.x + width/2, newY-margin) : new Vector2(-1000, -1000);
				left = new Node(new Vector2(start.x, start.y), new Vector2(end.x, newY-margin), doors);
				center = (corridor)? new Node(new Vector2(start.x, newY-margin), new Vector2(end.x, newY + margin), doors) : null;
				right = new Node(new Vector2(start.x, newY+margin), new Vector2(end.x, end.y), doors);
			}
		}

		public float getArea() {
			float width = Mathf.Abs (start.x - end.x);
			float length = Mathf.Abs (start.y - end.y);

			return width * length;
		}

		public bool isLeaf() {
			return left == null && right == null && center == null;
		}
	}

	public int FLOOR_WIDTH = 100;
	public int FLOOR_LENGTH = 100;

	//max chunk size with a number of rooms
	public float MAX_CHUNK_SIZE = 0.25f;

	//max room size in percentage of floor size
	public float MAX_ROOM_SIZE = 0.15f;

	//what is a chance that we will not split chunk into smaller rooms
	public float NO_ROOM_SPLIT_CHANCE = 0.15f;

	public int NO_OF_STAMPS = 10;
	private int alreadyPlacedStamps = 0;

	public RoomManager roomManager;

	public void setRoomManager(RoomManager roomManager) {
		this.roomManager = roomManager;
	}

	public void generateCorridors(Node root, bool rotated) {
		float floorArea = FLOOR_WIDTH * FLOOR_LENGTH;
		if (!root.isLeaf()) {
			if (root.left != null) {
				generateCorridors (root.left, !rotated);
			}
			if (root.right != null) {
				generateCorridors (root.right, !rotated);
			}
		} else {
			if (floorArea * MAX_CHUNK_SIZE <= root.getArea ()) {
				root.split (rotated, true, true);
				generateCorridors (root.left, !rotated);
				generateCorridors (root.right, !rotated);
			}
		}
	}

	public void generateRooms(Node root, bool rotated) {
		float floorArea = FLOOR_WIDTH * FLOOR_LENGTH;
		if (!root.isLeaf()) {
			if (root.left != null) {
				generateRooms (root.left, !rotated);
			}
			if (root.right != null) {
				generateRooms (root.right, !rotated);
			}
		} else {
			if (floorArea * MAX_ROOM_SIZE <= root.getArea () && Random.value > NO_ROOM_SPLIT_CHANCE) {
				root.split (rotated, false, true);
				generateRooms (root.left, !rotated);
				generateRooms (root.right, !rotated);
			}
		}
	}

	public void drawRooms(Node root) {
		if (!root.isLeaf ()) {
			if (root.left != null) {
				drawRooms (root.left);
			}
			if (root.center != null) {
				drawRooms (root.center);
			}
			if (root.right != null) {
				drawRooms (root.right);
			}
		} else {
			float width = Mathf.Abs (root.start.x - root.end.x);
			float length = Mathf.Abs (root.start.y - root.end.y);
			roomManager.generateRoom (width, length, root.start, root.doors);

			if (alreadyPlacedStamps < NO_OF_STAMPS) {
				roomManager.placeStamp (new Vector3 (root.start.x + width / 2, 1, root.start.y + length / 2));
			}

			RoomInfo roomInfo = new RoomInfo ();
			roomInfo.position = new Vector2 (root.start.x, root.start.y);
			roomInfo.size = new Vector2 (width, length);
			rooms.Add (roomInfo);
		}
	}

	public void generateFloor() {
		rooms = new List<RoomInfo> ();
		Node root = new Node(new Vector2(0, 0), new Vector2(FLOOR_WIDTH, FLOOR_LENGTH), new Vector2(-1000, -1000));
		generateCorridors (root, false);
		generateRooms (root, false);
		drawRooms (root);
	}
}
