using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private int count;

	public Text countText;
	public Text winText;

	public int NO_OF_STAMPS = 10;

	// Use this for initialization
	void Start () {
		count = 0;	
		winText.text = "";

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Stamp"))
		{
			Destroy (other.gameObject);
			count = count + 1;
			SetCountText ();
		}
	}


	void SetCountText ()
	{
		countText.text = "Count: " + count.ToString ();
		if (count >= NO_OF_STAMPS)
		{
			winText.text = "Win!";
		}
	}
}
