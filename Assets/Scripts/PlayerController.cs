using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private int count;
	private int total;

    private int minutes;
    private int seconds;
    private int miliseconds;

	public Text countText;
	public Text winText;
    public Text timeText;

	public int NO_OF_STAMPS = 10;

	private float SCALE_RATIO = 0.75f;
	public Sprite playerSprite; 

	bool stopTime = false;

	// Use this for initialization
	void Start () {
		count = 0;	
		total = 0;
        minutes = 0;
        seconds = 0;
        miliseconds = 0;
		winText.text = "";
	}

	void Awake() {
		SetCountText ();
        SetTimeText ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!stopTime) {
			UpdateTime ();
		}
        SetTimeText ();
	}

	public void AddStamp() {
		count = count + 1;
		int newTotal = GameObject.FindGameObjectsWithTag("Stamp").Length;
		if (newTotal > total) {
			total = newTotal;
		}
		SetCountText ();
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

	public bool AreAllStampsCollected() {
		return (count >= total);
	}

	void SetCountText ()
	{
		countText.text = "Count:" + count.ToString () + "/" + total.ToString();
		if (count >= total)
		{
			winText.text = "Run to the elevator!!!";
		}
	}

	public void LevelOver() {
		winText.text = "DONE!";
		stopTime = true;
	}

    void UpdateTime ()
    {
        minutes = (int)Time.time / 60;
        seconds = (int)Time.time % 60;
        miliseconds = (int)(Time.time * 100) % 100;
    }

    void SetTimeText ()
    {
        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds); 
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
