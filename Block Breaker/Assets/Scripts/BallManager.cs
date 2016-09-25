using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : MonoBehaviour {

	// required for creating or recreating balls
	public Ball ballPrefab;

	private List<Ball> balls;

	// Use this for initialization
	void Start () {
		balls = new List<Ball>(GameObject.FindObjectsOfType<Ball> ());
	}
	
	public void LaunchAll() {
		foreach (Ball b in balls) {
			b.Launch ();
		}
	}

	public void StopAll() {
		foreach (Ball b in balls) {
			b.GetComponent<Rigidbody2D> ().velocity = new Vector3(0f,0f,0f);
			b.GetComponent<Rigidbody2D> ().gravityScale = 0;
		}
	}
	public Ball makeNewBallAtPaddle(Paddle p){
		Ball b = (Ball)Instantiate (ballPrefab);
		b.name = "Ball";
		b.SetForStart (p);
		balls.Add (b);
		return b;
	}

	public void DestroyBall(Ball b) {
		balls.Remove (b);
		Destroy (b.gameObject);
	}

	public int getBallCount() {
		return balls.Count;
	}
}
