using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	[TextArea (3,5)]
	public string BallLossMessageText;

	[TextArea (3,5)]
	public string LevelWonMessageText;

	[TextArea (3,5)]
	public string LostFinalBallMessageText;

	private GameObject textFlashObj;
	private GameObject textLivesObj;

	public enum MESSAGE_EVENTS {LOST_BALL, WON_LEVEL, LOST_LAST_BALL};


	// Use this for initialization
	void Start () {
		textFlashObj = GameObject.FindGameObjectWithTag("MessageFlashBox");
		textFlashObj.GetComponent<Text>().text = "";
		textLivesObj = GameObject.FindGameObjectWithTag("LivesCounter");
		textLivesObj.GetComponent<Text>().text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ClearAllMessages() {
		textFlashObj.GetComponent<Text> ().text = "";
	}

	public void ShowMessageForEvent(MESSAGE_EVENTS e) {
		switch (e) {
		case MESSAGE_EVENTS.LOST_BALL:
			handleBallLossMessageEvent ();
			break;
		case MESSAGE_EVENTS.WON_LEVEL:
			handleLevelWonMessageEvent ();
			break;
		case MESSAGE_EVENTS.LOST_LAST_BALL:
			handleGameLostMessageEvent ();
			break;

		}
	}

	public void handleBallLossMessageEvent(){
		textFlashObj.GetComponent<Text>().text = BallLossMessageText;
	}

	public void handleLevelWonMessageEvent(){
		textFlashObj.GetComponent<Text>().text = LevelWonMessageText;
	}

	public void handleGameLostMessageEvent(){
		textFlashObj.GetComponent<Text>().text = LostFinalBallMessageText;
	}

	public void updateLivesText(int lives) {
		textLivesObj.GetComponent<Text>().text = "Lives: " + lives;
	}

}
