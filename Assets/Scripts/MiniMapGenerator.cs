using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniMapGenerator : MonoBehaviour {
	public Sprite tile;

	private List<Room> rooms;
	private int i = 0;

	public void generateMiniMap(List<Room> rooms) {
		this.rooms = rooms;
		Debug.Log (rooms.Count);
	}

	void drawTile(Vector3 position, Vector2 size) {
		Sprite s = tile;

		Texture t = s.texture;
		Rect tr = s.textureRect;
		Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height );

		float x = position.x/Config.SCALE_RATIO;
		float y = position.z/Config.SCALE_RATIO;
		float width = size.x/Config.SCALE_RATIO;
		float height = size.y/Config.SCALE_RATIO;
		GUI.DrawTextureWithTexCoords(new Rect(x, y, width, height), s.texture, r);
	}

	void OnGUI() {
		if (rooms != null) {
			foreach (Room room in rooms) {
				drawTile (room.position, room.size);
			}
		}
	}
}
