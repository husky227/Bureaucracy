using UnityEngine;
using System.Collections;

public class StampCollector : MonoBehaviour {
	GameObject player;
	PlayerController playerController;
	public Sprite stampSprite; 

	bool active = true;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectsWithTag ("Player") [0];
		playerController = (PlayerController) player.GetComponent(typeof(PlayerController));
	}

	// Update is called once per frame
	void Update () {
		if (active) {
			var distance = Vector3.Distance (transform.position, player.transform.position);
			if (distance < 2.5f) {
				playerController.AddStamp ();
				active = false;
				if (GetComponent<AudioSource>() != null) {
					GetComponent<AudioSource>().Play();
					GetComponent<Renderer>().enabled = false;
					Object.Destroy (this.transform.parent.gameObject, GetComponent<AudioSource>().clip.length);
				}
			}
		}
	}


	void OnGUI() {
		Sprite s = stampSprite;

		Texture t = s.texture;
		Rect tr = s.textureRect;
		Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height );

		float x = this.transform.position.x/Config.SCALE_RATIO;
		float y = this.transform.position.z/Config.SCALE_RATIO;
		float width = 2/Config.SCALE_RATIO;
		float height = 2/Config.SCALE_RATIO;

		GUI.DrawTextureWithTexCoords(new Rect(x, y, width, height), s.texture, r);
	}
}
