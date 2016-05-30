using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCorridorFloorGenerator : MonoBehaviour
{
	public Corridor corridor;
	public List<Room> rooms = new List<Room>();

	System.Random roomRandom;

	int corridorSalt = 123;
	int roomsSalt = 151900;

	public OneCorridorFloorGenerator ()
	{
	}

	public void generateFloor() {
		roomRandom = new System.Random (roomsSalt);

		corridor = new Corridor ();
		Debug.Log (corridor);
		corridor.randomizeCorridor (corridorSalt);

		if (!corridor.openSides.east) {
			generateEastSideRooms ();
		}
		if (!corridor.openSides.west) {
			generateWestSideRooms ();
		}
		if (!corridor.openSides.north) {
			generateNorthSideRooms ();
		}
		if (!corridor.openSides.south) {
			generateSouthSideRooms ();
		}
	}

	public void generateEastSideRooms() {
		generateVerticalSide (Side.EAST);
	}

	public void generateWestSideRooms() {
		generateVerticalSide (Side.WEST);
	}

	public void generateNorthSideRooms() {
		generateHorizontalSide (Side.NORTH);
	}

	public void generateSouthSideRooms() {
		generateHorizontalSide (Side.SOUTH);
	}

	public void generateVerticalSide(Side side) {
		float sumLength = 0;
		while (sumLength < corridor.size.y) {
			Room room = new Room ();
			room.doorSide = (side == Side.WEST)? Side.EAST : Side.WEST;
			float width = roomRandom.Next (Config.MIN_ROOM_WIDTH, Config.MAX_ROOM_WIDTH);
			float length = roomRandom.Next (Config.MIN_ROOM_WIDTH, Config.MAX_ROOM_WIDTH);
			room.size = new Vector2 (width, length);
			float x = (side == Side.WEST)? corridor.position.x - width : corridor.position.x + corridor.size.x ; 
			float y = 0;
			float z = corridor.position.z + sumLength;
			room.position = new Vector3 (x, y, z);
			room.doorPosition = roomRandom.Next(Mathf.FloorToInt(Config.DOOR_WIDTH/2), Mathf.FloorToInt(length - Config.DOOR_WIDTH));
			if (sumLength + room.doorPosition > corridor.size.y) {
				//room doors outside of corridor
				room.doorPosition = Mathf.FloorToInt(Config.DOOR_WIDTH/2);
			}
			sumLength += length;
			rooms.Add (room);
		}
	}

	public void generateHorizontalSide(Side side) {
		float sumLength = 0;
		while (sumLength < corridor.size.x) {
			Room room = new Room ();
			room.doorSide = (side == Side.NORTH)? Side.SOUTH : Side.NORTH;
			float width = roomRandom.Next (Config.MIN_ROOM_WIDTH, Config.MAX_ROOM_WIDTH);
			float length = roomRandom.Next (Config.MIN_ROOM_WIDTH, Config.MAX_ROOM_WIDTH);

			if (length + sumLength > corridor.size.x) {
				length = corridor.size.x - sumLength;
			}

			room.size = new Vector2 (width, length);
			float x = corridor.position.x + sumLength;
			float y = 0;
			float z = (side == Side.NORTH)? corridor.position.y - length : corridor.position.y + corridor.size.y; 
			room.position = new Vector3 (x, y, z);
			room.doorPosition = roomRandom.Next(Mathf.FloorToInt(Config.DOOR_WIDTH/2), Mathf.FloorToInt(width - Config.DOOR_WIDTH));
			if (sumLength + room.doorPosition > corridor.size.x) {
				//room doors outside of corridor
				room.doorPosition = 0;
			}
			//do not overflow - there might some vertical side room
			if (sumLength + width > corridor.size.x) {
				break;
			}
			sumLength += width;
			rooms.Add (room);
		}
	}
}
