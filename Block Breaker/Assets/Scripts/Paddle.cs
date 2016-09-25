using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Paddle : MonoBehaviour {

	public float MoveFactor;
	public bool autoPlay = false;
	public AudioClip bounce;

	public float paddleLeftLimit, paddleRightLimit;

	private float mousePosInBlocks;
	private Vector3 paddlePos;

	private Ball ball;

	void Start(){
		paddlePos = new Vector3 (0.5f, this.transform.position.y, 0f);
	}

	// Update is called once per frame
	void Update () {
		if (!autoPlay) {
			MoveWithMouse ();
		} else {
			AutoPlay ();
		}
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag.Equals ("Ball")) {
			AudioSource.PlayClipAtPoint (bounce, transform.position, 0.8f);
		}

	}
	void AutoPlay(){
		if (ball == null) {
			ball = GameObject.FindObjectOfType<Ball> (); 
		}
		paddlePos.x = Mathf.Clamp(ball.transform.position.x, paddleLeftLimit, paddleRightLimit);
		this.transform.position = paddlePos;

	}

	void MoveWithMouse(){
		mousePosInBlocks = Mathf.Clamp(Input.mousePosition.x / Screen.width * 16, paddleLeftLimit, paddleRightLimit);
		paddlePos = this.transform.position;
		paddlePos.x = mousePosInBlocks;
		this.transform.position = paddlePos;
	}
}
