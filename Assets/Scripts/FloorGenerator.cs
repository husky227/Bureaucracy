using UnityEngine;
using System.Collections;

public class FloorGenerator : MonoBehaviour {
	private const float SPLIT_RANDOM_RANGE_MIN = 0.35f;
	private const float SPLIT_RANDOM_RANGE_MAX = 0.65f;

	private const int CORRIDOR_WIDTH = 4;

	public class Node {
		public Node left = null;
		public Node right = null;

		public Vector2 start;
		public Vector2 end;

		public Node(Vector2 start, Vector2 end) {
			this.start = start;
			this.end = end;
		}

		public void split(bool rotated, bool corridor) {
			float width = Mathf.Abs (start.x - end.x);
			float length = Mathf.Abs (start.y - end.y);

			float splitted = Random.Range(SPLIT_RANDOM_RANGE_MIN*100, SPLIT_RANDOM_RANGE_MAX*100)/100.0f;

			float margin = (corridor) ? CORRIDOR_WIDTH / 2 : 0;

			if (rotated) {
				//split width
				float newX = start.x + width*splitted;
				left = new Node(new Vector2(start.x, start.y), new Vector2(newX-margin, end.y));
				right = new Node(new Vector2(newX+margin, start.y), new Vector2(end.x, end.y));
			} else {
				//split length
				float newY = start.y + length*splitted;
				left = new Node(new Vector2(start.x, start.y), new Vector2(end.x, newY-margin));
				right = new Node(new Vector2(start.x, newY+margin), new Vector2(end.x, end.y));
			}
		}

		public float getArea() {
			float width = Mathf.Abs (start.x - end.x);
			float length = Mathf.Abs (start.y - end.y);

			return width * length;
		}

		public bool isLeaf() {
			return left == null && right == null;
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

	public RoomManager roomManager;

	public void setRoomManager(RoomManager roomManager) {
		this.roomManager = roomManager;
	}

	public void generateCorridors(Node root, bool rotated) {
		float floorArea = FLOOR_WIDTH * FLOOR_LENGTH;
		if (!root.isLeaf()) {
			if (root.left != null) {
				generateRooms (root.left, !rotated);
			}
			if (root.right != null) {
				generateRooms (root.right, !rotated);
			}
		} else {
			if (floorArea * MAX_CHUNK_SIZE <= root.getArea ()) {
				root.split (rotated, true);
				generateRooms (root.left, !rotated);
				generateRooms (root.right, !rotated);
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
				root.split (rotated, false);
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
			if (root.right != null) {
				drawRooms (root.right);
			}
		} else {
			float width = Mathf.Abs (root.start.x - root.end.x);
			float length = Mathf.Abs (root.start.y - root.end.y);
			roomManager.generateRoom (width, length, root.start);
		}
	}

	public void generateFloor() {
		Node root = new Node(new Vector2(0, 0), new Vector2(FLOOR_WIDTH, FLOOR_LENGTH));
		generateCorridors (root, false);
		generateRooms (root, false);
		drawRooms (root);
	}
}
