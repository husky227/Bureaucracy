using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class FloorGenerator
	{
		Corridor corridor;
		List<Room> rooms = new List<Room>();

		System.Random roomRandom;

		int corridorSalt = 42;
		int roomsSalt = 151900;

		public FloorGenerator ()
		{
			roomRandom = new System.Random ();

			corridor = new Corridor ();
			corridor.randomizeCorridor (42);

			if (corridor.openSides.east) {
				generateEastSideRooms ();
			}
			if (corridor.openSides.west) {
				generateWestSideRooms ();
			}
			if (corridor.openSides.north) {
				generateNorthSideRooms ();
			}
			if (corridor.openSides.south) {
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
				room.doorSide = side;
				float width = roomRandom.Next (Config.MIN_ROOM_WIDTH, Config.MAX_ROOM_WIDTH);
				float length = roomRandom.Next (Config.MIN_ROOM_WIDTH, Config.MAX_ROOM_WIDTH);
				room.size = new Vector2 (width, length);
				float x = (side == Side.EAST)? corridor.position.x - width : corridor.position.x + corridor.size.x ; 
				float y = 0;
				float z = corridor.position.z - sumLength;
				room.position = new Vector3 (x, y, z);
				room.doorPosition = roomRandom.NextDouble(0, length-Config.DOOR_WIDTH);
				if (sumLength + room.doorPosition > corridor.size.y) {
					//room doors outside of corridor
					room.doorPosition = 0;
				}
				sumLength += length;
				rooms.Add (room);
			}
		}

		public void generateHorizontalSide(Side side) {
			float sumLength = 0;
			while (sumLength < corridor.size.x) {
				Room room = new Room ();
				room.doorSide = side;
				float width = roomRandom.Next (Config.MIN_ROOM_WIDTH, Config.MAX_ROOM_WIDTH);
				float length = roomRandom.Next (Config.MIN_ROOM_WIDTH, Config.MAX_ROOM_WIDTH);

				if (length + sumLength > corridor.size.x) {
					length = corridor.size.x - sumLength;
				}

				room.size = new Vector2 (width, length);
				float x = corridor.position.x - sumLength;
				float y = 0;
				float z = (side == Side.NORTH)? corridor.position.y - length : corridor.position.y + corridor.size.y; 
				room.position = new Vector3 (x, y, z);
				room.doorPosition = roomRandom.NextDouble(0, length-Config.DOOR_WIDTH);
				if (sumLength + room.doorPosition > corridor.size.x) {
					//room doors outside of corridor
					room.doorPosition = 0;
				}
				sumLength += length;
				rooms.Add (room);
			}
		}
	}
}

