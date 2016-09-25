using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

/** 
 * Is responsible for managing the level and passing messages to subcomponents for handling of common tasks.
 * */
public class LevelManager : MonoBehaviour {

	private GameDirector gameDirector;

	void Start(){
		gameDirector = GameObject.FindObjectOfType<GameDirector> ();
	}

	public void LoadLevel(string name){
		Brick.breakableCount = 0;
		GameDirector.PlayerLives = GameDirector.NUM_LIVES;
		SceneManager.LoadScene(name);
	}

	public void LoadNextLevel() {
		Brick.breakableCount = 0;
		GameDirector.PlayerLives = GameDirector.NUM_LIVES;
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}
		
	public void BrickDestroyedMessage() {
		if (Brick.breakableCount <= 0) {
			gameDirector.handleEndOfLevel();
		}
	}

	/**
	 * Handles objects falling through the trigger at the bottom of the screen
	 */
	public void FallThroughMessage(Collider2D collider) {
		if (collider.gameObject.tag.Equals ("Ball")) {
			gameDirector.handleBallLoss (collider.gameObject.GetComponent<Ball>());
		}
	}

	public void QuitRequest() {
		print ("Quit requested");
		// BAD CALL -->>> IOS REJECTS APPS WITH Application.Quit ();
	}


}
