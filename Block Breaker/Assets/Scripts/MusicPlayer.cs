using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** 
 * Singleton Music player to act as Audio Source for level audio.
 */
public class MusicPlayer : MonoBehaviour {


	// singleton MusicPlayer - keep it as demo implementation.
	static MusicPlayer _instance = null;

	void Awake() {
		//Lazy instantiate
		if (_instance != null) {
			Debug.Log ("Music Player Destroy: " + GetInstanceID ());
			Destroy (gameObject);
		} else {
			_instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}
		
	void LoadClip(AudioClip c) {
		GetComponent<AudioSource> ().clip = c;
	}

	void Play() {
		GetComponent<AudioSource> ().Play();
	}

	void Stop() {
		GetComponent<AudioSource> ().Stop();
	}

}
