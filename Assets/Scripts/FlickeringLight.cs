using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour {

	Light[] lights;
	bool flickers = false;
	// Use this for initialization
	void Start () {
		lights = GetComponentsInChildren<Light> ();
		flickers = Random.value > 0.7;
	}

	void Update () 
	{
		if (flickers) {
			if (Random.value > 0.9) {
				if (lights [0].enabled == true) {
					lights [0].enabled = false;
					lights [1].enabled = false;
				} else {
					lights [0].enabled = true;
					lights [1].enabled = true;
				}
			}
		}
	}
}
