using UnityEngine;
using System.Collections;

public class Room
{
	public Vector3 position { get; set; }
	public Vector3 size { get; set; }
	public Side doorSide { get; set; }
	public float doorPosition = 0; //relative to doorSide wall

	public enum RoomType
	{
		BATHROOM, STORAGE_ROOM, OFFICE
	}

	public Room() {
	}

	public void generateRoom(Vector3 position, Vector2 size, Side doorSide) {
		this.position = position;
		this.size = size;
		this.doorSide = doorSide;
	}
}

