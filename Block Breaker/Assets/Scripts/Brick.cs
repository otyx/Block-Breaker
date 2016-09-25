using UnityEngine;
using System.Collections; 

public class Brick : MonoBehaviour {

	public AudioClip explode;
	public AudioClip damage;

	public Sprite[] hitSprites;
	public GameObject smoke;

	public static int breakableCount = 0;

	//private vars - don't peak!
	private int timesHit;
	private int maxHits;
	private LevelManager levelManager;
	private bool isBreakable;
	private GameObject particles = null;

	// Use this for initialization
	void Start () {
		isBreakable = (this.tag == "Breakable");
		if (isBreakable) {
			Brick.breakableCount++;
		}

		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		timesHit = 0;
		maxHits = hitSprites.Length + 1;
	}

	void Update() {
		if (particles && !particles.GetComponent<ParticleSystem>().IsAlive()) {
			Destroy (particles);
		}

	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (this.tag.Equals("Breakable")) {
			HandleHits ();
		}
	}

	void HandleHits() {
		timesHit++;
		if (timesHit >= maxHits) {
			AudioSource.PlayClipAtPoint (explode, transform.position, 0.8f);
			SelfDestruct ();
		} else {
			AudioSource.PlayClipAtPoint (damage, transform.position, 0.8f);
			LoadSprites ();
		}
	}

	void PuffSmoke(){
		particles = (GameObject)Instantiate (smoke, gameObject.transform.position, Quaternion.identity);
		particles.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer> ().color;
		Destroy (particles, particles.GetComponent<ParticleSystem> ().startLifetime + particles.GetComponent<ParticleSystem> ().duration);
	}

	void SelfDestruct(){
		Brick.breakableCount--;
		PuffSmoke ();
		Destroy (gameObject);
		levelManager.BrickDestroyedMessage ();
	}


	void LoadSprites(){
		int spriteIndex = timesHit - 1;
		if (hitSprites [spriteIndex]) {
			this.GetComponent<SpriteRenderer> ().sprite = hitSprites [spriteIndex];
		} else {
			Debug.LogError ("Brick - LoadSprites() : Missing Sprite for index: " + spriteIndex);
		}
	}

}
