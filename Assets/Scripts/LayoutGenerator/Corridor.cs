using UnityEngine;
using System.Collections;

public class Corridor : MonoBehaviour {
	public class OpenSides {
		public bool west = false;
		public bool east = false;
		public bool south = false;
		public bool north = false;
	}

	public enum CorridorType
	{
		HALL, TWO_SIDE, THREE_SIDE, ONE_SIDE
	}

	public Vector3 position { get; set; }
	public Vector2 size { get; set; }
	public OpenSides openSides { get; set; }
	public CorridorType corridorType;

	public Corridor ()
	{
	}

	public void randomizeCorridor(int seed) {
		System.Random random = new System.Random (seed);

		position = new UnityEngine.Vector3 (0, 0, 0);
		size = new UnityEngine.Vector2 (0, 0);

		var v = CorridorType.GetValues ();
		corridorType = v.GetValue (random.Next (v.Length));

		int width = 0;
		int length = 0;

		switch (corridorType) {
		case CorridorType.HALL:
			width = random.Next (Config.MIN_CORRIDOR_WIDTH, Config.MAX_CORRIDOR_WIDTH);
			length = width;
			break;
		default:
			width = random.Next (Config.MIN_CORRIDOR_WIDTH, Config.MAX_CORRIDOR_WIDTH);
			length = Mathf.FloorToInt(Mathf.Min(random.Next (Config.MIN_CORRIDOR_WIDTH, Config.MAX_CORRIDOR_WIDTH), width/2));
			break;
		}

		size.Set (width, length);

		openSides = new OpenSides ();
		switch (corridorType) {
		case CorridorType.HALL:
			openSides.east = true;
			openSides.west = true;
			openSides.north = true;
			openSides.south = true;
			break;
		case CorridorType.ONE_SIDE:
			if (random.NextDouble () > 0.5) {
				openSides.north = true;
				openSides.south = false;
			} else {
				openSides.north = false;
				openSides.south = true;
			}
			openSides.east = false;
			openSides.west = false;
			break;
		case CorridorType.TWO_SIDE:
			openSides.north = true;
			openSides.south = true;
			openSides.east = false;
			openSides.west = false;
			break;
		case CorridorType.THREE_SIDE:
			if (random.NextDouble () > 0.5) {
				openSides.north = true;
				openSides.south = false;
			} else {
				openSides.north = false;
				openSides.south = true;
			}
			openSides.east = true;
			openSides.west = true;
			break;
		}

	}
}

