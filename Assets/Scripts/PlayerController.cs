using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private int count;

	public Text countText;
	public Text winText;

	public int NO_OF_STAMPS = 10;

	private float SCALE_RATIO = 0.75f;
	public Sprite playerSprite; 

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

	void OnGUI() {
		Sprite s = playerSprite;

		Texture t = s.texture;
		Rect tr = s.textureRect;
		Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height );

		float x = this.transform.position.x/SCALE_RATIO;
		float y = this.transform.position.z/SCALE_RATIO;
		float width = 10/SCALE_RATIO;
		float height = 10/SCALE_RATIO;

		GUI.DrawTextureWithTexCoords(new Rect(x, y, width, height), s.texture, r);
	}
}
