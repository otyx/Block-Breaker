using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * This is responsible for all things audio. All sounds effects are handled here.
 */
public class AudioDirector : MonoBehaviour {

	#region Singleton
	// Singleton instance
	private static AudioDirector _instance;
	void Awake () {
		//Lazy instantiate
		if (_instance != null) {
			Debug.Log ("AudioDirector Destroy: " + GetInstanceID ());
			Destroy (gameObject);
		} else {
			_instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}
	#endregion

	public AudioClip loseGame;
	public AudioClip winGame;
	public AudioClip dropBall;
	public AudioClip loseLevel;
	public AudioClip winLevel;
	public AudioClip themeMusic;
	public AudioClip brickBounce;
	public AudioClip brickBreak;
	public AudioClip unbreakableBounce;
	public AudioClip ballBounce;

	public enum GameAudioClips {THEME_MUSIC, WIN_LEVEL, LOSE_LEVEL, WIN_GAME, LOSE_GAME, DROP_BALL, BRICK_BOUNCE, BRICK_BREAK, UNBREAKABLE_BOUNCE, BALL_BOUNCE};

	public Dictionary<GameAudioClips, AudioClip> AudioClips = new Dictionary<GameAudioClips, AudioClip> ();


	private AudioSource _musicAudioSource;
	private AudioSource _effectAudioSource;


	/**
	 * Loads the Audio Clips into a Dictionary structure accessible using the enum keys
	 */
	public void initialiseAudio() {
		// now pre-load the clips
		AudioClips.Add(	GameAudioClips.THEME_MUSIC, 		themeMusic			);
		AudioClips.Add(	GameAudioClips.WIN_LEVEL, 			winLevel			);
		AudioClips.Add(	GameAudioClips.LOSE_LEVEL, 			loseLevel			);
		AudioClips.Add(	GameAudioClips.WIN_GAME, 			winGame				);
		AudioClips.Add(	GameAudioClips.LOSE_GAME, 			loseGame			);
		AudioClips.Add(	GameAudioClips.DROP_BALL, 			dropBall			);
		AudioClips.Add(	GameAudioClips.BRICK_BOUNCE, 		brickBounce			);
		AudioClips.Add(	GameAudioClips.UNBREAKABLE_BOUNCE, 	unbreakableBounce	);
		AudioClips.Add(	GameAudioClips.BRICK_BREAK, 		brickBreak			);
		AudioClips.Add(	GameAudioClips.BALL_BOUNCE, 		ballBounce			);

		// load the audio sources
		_musicAudioSource 	= GameObject.FindGameObjectWithTag("MusicAudioSource").GetComponent<AudioSource>();
		_effectAudioSource 	= GameObject.FindGameObjectWithTag("EffectsAudioSource").GetComponent<AudioSource>();
	}


	void Start() {
		initialiseAudio ();
	}
		
	public void PlayMusic (GameAudioClips c, bool loop=true, float pitch = 1.0f, float volume=1 ) {
		_musicAudioSource.clip = AudioClips [c];
		_musicAudioSource.loop = loop;
		_musicAudioSource.volume = volume;
		_musicAudioSource.pitch = pitch;
		_musicAudioSource.Play ();
	}

	public void StopMusic() {
		_musicAudioSource.Stop ();
	}

	public void StopEffects() {
		_effectAudioSource.Stop ();
	}

	public void RestartMusic() {
		Restart (_musicAudioSource);
	}

	public void RestartEffect() {
		Restart (_effectAudioSource);
	}

	public void PlayEffect(GameAudioClips c, bool loop=true, float pitch = 1.0f, float volume=1 ) {
		_effectAudioSource.clip = AudioClips [c];
		_effectAudioSource.loop = loop;
		_effectAudioSource.volume = volume;
		_effectAudioSource.pitch = pitch;
		_effectAudioSource.Play ();
	}

	public void PlayEffectClip(GameAudioClips c) {
		PlayEffectClipAt (c, new Vector3 ());
	}

	public void PlayEffectClipAt(GameAudioClips c, Vector3 position) {
		AudioSource.PlayClipAtPoint(AudioClips [c], position);
	}

	public bool isPlaying(GameAudioClips c) {
		bool result = false;
		if (_musicAudioSource.clip == AudioClips [c] && _musicAudioSource.isPlaying) {
			result = true;
		} else if (_effectAudioSource.clip == AudioClips [c] && _effectAudioSource.isPlaying) {
			result = true;
		}
		return result;
	}

	#region Private Methods
	private void Restart(AudioSource src) {
		if (src.clip) {
			src.Play ();
		} else {
			Debug.LogWarning ("AD: Restart - No clip loaded (" + src.name + ")");
		}
	}

	#endregion

}
