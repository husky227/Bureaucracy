using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {
	GameObject doors;
	private bool closed = true;

	// Use this for initialization
	void Start () {
		Transform t = transform;
		foreach(Transform tr in t)
		{
			if(tr.tag.Equals("Door"))
			{
				doors = tr.gameObject;
				return;
			}
		}
	}

	void toggleDoors() {
		Vector3 hinge = new Vector3 (transform.position.x, transform.position.y, transform.position.z + Config.DOOR_WIDTH / 2);
		if (!closed) {
			doors.transform.RotateAround (hinge, new Vector3 (0, 1, 0), 90);
		} else {
			doors.transform.RotateAround (hinge, new Vector3 (0, 1, 0), -90);
		}
		closed = !closed;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E)) {
			GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
			var distance = Vector3.Distance(transform.position, player.transform.position);
			if (distance < 5f) {
				toggleDoors();
			}
		}
	}
}
