using UnityEngine;
using System.Collections;

public class ElevatorController : MonoBehaviour {
	GameObject player;
	PlayerController playerController;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectsWithTag ("Player") [0];
		playerController = (PlayerController) player.GetComponent(typeof(PlayerController));
	}
	
	// Update is called once per frame
	void Update () {
		if (playerController.AreAllStampsCollected()) {
			var distance = Vector3.Distance (transform.position, player.transform.position);
			if (distance < 2.5f) {
				playerController.LevelOver ();
			}
		}
	}
}
