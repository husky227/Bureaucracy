using UnityEngine;
using System.Collections;

public class StampCollector : MonoBehaviour {
	GameObject player;
	PlayerController playerController;

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
}
