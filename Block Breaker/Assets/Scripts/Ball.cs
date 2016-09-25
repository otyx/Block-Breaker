using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	private GameDirector gameDirector;

	private float ballSpeed = 10f;

	private Paddle paddle;

	private Vector3 paddleToBallVector;

	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		gameDirector = GameObject.FindObjectOfType<GameDirector> ();
		paddleToBallVector = new Vector3 (0f,0.5f,0f);
	}

	public void SetForStart(Paddle p){
		paddle = p;
		transform.position = paddle.transform.position + paddleToBallVector;
	}

	// Update is called once per frame
	void Update () {
		if (GameDirector.isState(GameDirector.GAME_STATE.PLAYING)) {
			this.transform.Rotate(new Vector3 (0,0, transform.rotation.z - this.GetComponent<Rigidbody2D>().velocity.x ));
		} else {
			// clamp ball to paddle
			this.transform.position = paddle.transform.position + paddleToBallVector;
		}
	}

	/**
	 * Fire off the ball
	 */
	public void Launch() {
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.5f, ballSpeed);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//introduce a little randomness into the bounces
		Vector2 tweak = new Vector2 (Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
		if (GameDirector.isState(GameDirector.GAME_STATE.PLAYING)) {	
			GetComponent<Rigidbody2D>().velocity += tweak;
		}
	}


	public float BallSpeed {
		get {
			return ballSpeed;
		}
	}
}
