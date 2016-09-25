using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/**
 * The Game Director orchestrates the entire game flow. It instructs level manager to load levels, manages game states
 * */
public class GameDirector : MonoBehaviour {

	/*#region Singleton
	// Singleton instance
	private static GameDirector _instance;
	void Awake () {
		//Lazy instantiate
		if (_instance != null) {
			Debug.Log ("*** GameDirector Destroy: " + GetInstanceID ());
			Destroy (gameObject);
		} else {
			_instance = this;
			Debug.Log ("***  GameDirector Created: " + GetInstanceID ());
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}
	#endregion
*/
	public enum GAME_STATE
	{
		MENU_PAUSED,
		LOAD_LEVEL,
		LAUNCH_READY,
		PLAYING,
		PAUSED,
		LOST,
		GAME_WON,
		LEVEL_WON
	};

	public const int NUM_LIVES = 3;
	public static int PlayerLives = NUM_LIVES;

	private static GAME_STATE gameState = GAME_STATE.MENU_PAUSED;
	private static string _nextLevelToLoad;

	private static LevelManager 	levelManager;
	private static BallManager 		ballManager;
	private static MusicPlayer 		musicPlayer;
	private static AudioDirector 	audioDirector;
	private static UIManager		uiManager;
	private static Paddle 			paddle;

	// Use this for initialization
	void Start() {
		Debug.Log ("GD Start called");
		// assign the relevant managers
		levelManager 	= 	GameObject.FindObjectOfType<LevelManager>();
		ballManager 	= 	GameObject.FindObjectOfType<BallManager> ();
		musicPlayer 	= 	GameObject.FindObjectOfType<MusicPlayer> ();
		audioDirector	= 	GameObject.FindObjectOfType<AudioDirector> ();
		uiManager 		= 	GameObject.FindObjectOfType<UIManager> ();

		// start theme music if menu page
		if (gameState == GAME_STATE.MENU_PAUSED){
			audioDirector.PlayMusic (AudioDirector.GameAudioClips.THEME_MUSIC, true, 1.15f, 0.2f);
		}
	}

	// Update is called once per frame
	void Update () {
		Debug.Log ("GD Update: gameState: " + gameState );
		if (paddle == null) {
			paddle = GameObject.FindObjectOfType<Paddle>();
			Debug.Log ("GD - Paddle was null so tried to fetch: '" + paddle + "'");
		}
		if (ballManager == null) {
			ballManager = GameObject.FindObjectOfType<BallManager>();
			Debug.Log ("GD - ballManager was null so tried to fetch: '" + ballManager + "'");
		}
		if (uiManager == null) {
			uiManager = GameObject.FindObjectOfType<UIManager> ();
			Debug.Log ("GD - uiManager was null so tried to fetch: '" + uiManager + "'");
		}

		Debug.Log ("NextScene To Load is: " + _nextLevelToLoad);
		switch (gameState) {
		// on a menu page - let the GUI element deal with any inputs
		case GAME_STATE.MENU_PAUSED:
			// Start the menu music
			// button on menu will start the game...
			if (!audioDirector.isPlaying (AudioDirector.GameAudioClips.THEME_MUSIC)) {
				audioDirector.PlayMusic (AudioDirector.GameAudioClips.THEME_MUSIC);
			}
			break;
		// Prepare the level for play
		case GAME_STATE.LOAD_LEVEL:
			Debug.Log ("Ball Manager: " + ballManager + ", Paddle: " + paddle);
			if (ballManager != null && paddle != null) {
				ballManager.makeNewBallAtPaddle (paddle);
				TransitionToState (GAME_STATE.LAUNCH_READY);
			}
			break;
		// Game is paused due to an onscreen win/lose message - mouse click to proceed to end screen
		case GAME_STATE.PAUSED:
			if (!audioDirector.isPlaying (AudioDirector.GameAudioClips.THEME_MUSIC)) {
				audioDirector.RestartMusic ();
			}
			Cursor.visible = true;
			if (Input.GetMouseButtonDown (0)){
				Cursor.visible = false;
				levelManager.LoadLevel (_nextLevelToLoad);
			}
			break;
		// Game is playing - ignore mouse clicks
		case GAME_STATE.PLAYING:
			if (audioDirector.isPlaying (AudioDirector.GameAudioClips.THEME_MUSIC)) {
				audioDirector.StopMusic ();
			}
			break;
		// Game is ready for launch so listen for a mouse click
		case GAME_STATE.LAUNCH_READY:
			//Debug.Log ("Launch Ready GameState: " + _state);
			if (audioDirector.isPlaying (AudioDirector.GameAudioClips.THEME_MUSIC)) {
				audioDirector.StopMusic ();
			}
			if (Input.GetMouseButtonDown (0)){
				// Left mouse button is down.
				if (ballManager) {
					ballManager.LaunchAll ();
					Cursor.visible = false;
					uiManager.ClearAllMessages ();
					TransitionToState(GAME_STATE.PLAYING);
				} else {
					Debug.LogError ("GD Update: ERROR - No ball manager for mouse click launch ");
				}
			}
			break;
		case GAME_STATE.LOST:
			Cursor.visible = true;
			levelManager.LoadLevel ("Lose Scene");
			break;
		case GAME_STATE.LEVEL_WON:
			uiManager.ShowMessageForEvent (UIManager.MESSAGE_EVENTS.WON_LEVEL);
			TransitionToState (GAME_STATE.PAUSED);
			_nextLevelToLoad = SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1).name;
			Cursor.visible = true;
			break;
		// Shouldn't happen - this is a problem!
		default:
			Debug.LogError ("Unhandled GameState" + gameState);
			break;
		}
	}

	public void StartGame() {
		//TransitionToState (GAME_STATE.MENU_PAUSED);
		Brick.breakableCount = 0;
		GameDirector.PlayerLives = GameDirector.NUM_LIVES;
		levelManager.LoadLevel ("StartMenu");
		this.Start ();
	}

	public void StartLevel() {
		TransitionToState(GAME_STATE.LOAD_LEVEL);
		levelManager.LoadNextLevel ();
	}

	public void LaunchBall() {
		TransitionToState(GAME_STATE.PLAYING);
		uiManager.ClearAllMessages ();
		ballManager.LaunchAll ();
		Cursor.visible = false;
	}

	public void handleBallLoss(Ball ball) {
		// remove the ball from the game
		Debug.Log("GD DestroyBall and BallManager is " + ballManager);
		//if (ballManager == null) {
			ballManager = GameObject.FindObjectOfType<BallManager> ();
		//}
		ballManager.DestroyBall(ball);

		// check if it was the last ball
		if (ballManager.getBallCount() <= 0) {
			// if it was, then a life is lost
			PlayerLives--;
			//if (uiManager == null) {
				uiManager = GameObject.FindObjectOfType<UIManager> ();
			//}
			uiManager.updateLivesText(PlayerLives);

			if (PlayerLives <= 0) {
				// that's it - end of game...
				TransitionToState(GAME_STATE.PAUSED);
				uiManager.ShowMessageForEvent (UIManager.MESSAGE_EVENTS.LOST_LAST_BALL);
				_nextLevelToLoad = "Lose Scene";
			} else {
				// reset a new ball so we can continue playing
				// there is only one ball on the playfield now and any paddle buffs 
				// will be neutralised
			//	if (audioDirector == null) {
					audioDirector = GameObject.FindObjectOfType<AudioDirector> ();
			//	}
				audioDirector.PlayEffectClip(AudioDirector.GameAudioClips.DROP_BALL);
				uiManager.ShowMessageForEvent(UIManager.MESSAGE_EVENTS.LOST_BALL);
				TransitionToState (GAME_STATE.LAUNCH_READY);
			//	if (paddle == null) {
					paddle = GameObject.FindObjectOfType<Paddle>();
			//		Debug.Log ("GD - HandleBallLoss Paddle was null so tried to fetch: '" + paddle + "'");
			//	}
				ballManager.makeNewBallAtPaddle (paddle);
			}
		} else {
			// there are still balls in play... just continue playing...
			Debug.Log("Ball lost but continuing - there are still " + ballManager.getBallCount() + " balls in play...");
		}
	}

	public void handleEndOfLevel(){
		// move to the next level and continue!
		TransitionToState (GAME_STATE.LEVEL_WON);
	}

	public static bool isState(GAME_STATE s) {
		return gameState == s;
	}

	public void TransitionToState(GAME_STATE s) {
		gameState = s;
	}


}
