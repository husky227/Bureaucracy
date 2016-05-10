using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniMapGenerator : MonoBehaviour {
	public Sprite tile;
	private float SCALE_RATIO = 0.75f;
	private List<RoomInfo> rooms;
	private int i = 0;
	public void generateMiniMap(List<RoomInfo> rooms) {
		this.rooms = rooms;
	}

	void drawTile(Vector2 position, Vector2 size) {
		Sprite s = tile;

		Texture t = s.texture;
		Rect tr = s.textureRect;
		Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height );

		float x = position.x/SCALE_RATIO;
		float y = position.y/SCALE_RATIO;
		float width = size.x/SCALE_RATIO;
		float height = size.y/SCALE_RATIO;

		GUI.DrawTextureWithTexCoords(new Rect(x, y, width, height), s.texture, r);
	}

	void OnGUI() {
		foreach (RoomInfo room in rooms) {
			drawTile (room.position, room.size);
		}
	}
}
