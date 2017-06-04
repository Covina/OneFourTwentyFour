using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour {

	// Singleton
	public static AppController instance;

	// Sound On/Off
	private bool isSoundOn = true;
	public bool IsSoundOn {
		get {
			if (PlayerPrefs.HasKey ("Sound")) {

				return (PlayerPrefs.GetInt ("Sound") == 1) ? true : false;

			} else {
				// if they do not have it, return local var
				return true;
			}
		}
		set {
			PrefsSetSound(value);
		}


	}

	// Does the player's score qualify
	private bool playerScoreQualifies = false;
	public bool PlayerScoreQualifies {
		get {
			return playerScoreQualifies;
		}	
		set {
			playerScoreQualifies = value;
		}
	}


	// final player score
	private int playerFinalScore = 0;
	public int PlayerFinalScore {
		get {
			return playerFinalScore;
		}
		set {
			playerFinalScore = value;
		}
	}

	// Does CPU1 score qualify
	private bool cpu1ScoreQualifies = false;
	public bool CPU1ScoreQualifies {
		get {
			return cpu1ScoreQualifies;
		}	
		set {
			cpu1ScoreQualifies = value;
		}
	}


	// Final CPU1 score
	private int cpu1FinalScore = 0;
	public int CPU1FinalScore {
		get {
			return cpu1FinalScore;
		}
		set {
			cpu1FinalScore = value;
		}
	}


	// Does CPU2 score qualify
	private bool cpu2ScoreQualifies = false;
	public bool CPU2ScoreQualifies {
		get {
			return cpu2ScoreQualifies;
		}	
		set {
			cpu2ScoreQualifies = value;
		}
	}


	// Final CPU1 score
	private int cpu2FinalScore = 0;
	public int CPU2FinalScore {
		get {
			return cpu2FinalScore;
		}
		set {
			cpu2FinalScore = value;
		}
	}

	// Does CPU1 score qualify
	private bool cpu3ScoreQualifies = false;
	public bool CPU3ScoreQualifies {
		get {
			return cpu3ScoreQualifies;
		}	
		set {
			cpu3ScoreQualifies = value;
		}
	}


	// Final CPU1 score
	private int cpu3FinalScore = 0;
	public int CPU3FinalScore {
		get {
			return cpu3FinalScore;
		}
		set {
			cpu3FinalScore = value;
		}
	}

	// is this a new game starting?
	private bool isNewGame = true;
	public bool IsNewGame {
		get {
			return isNewGame;
		}
		set {
			isNewGame = value;
		}
	}


	void Awake ()
	{

		// Create Singleton
		if (instance == null) {

			instance = this;
		} else if (instance != this) {

			Destroy(gameObject);

		}

		DontDestroyOnLoad(instance);

	}

	// Load Scene Navigation
	public void LoadScene (string scene)
	{

		SceneManager.LoadScene(scene);

	}


	// Store the Sounds Setting in PlayerPrefs
	public void PrefsSetSound(bool newValue)
	{
		// convert bool to int
		int tmp = (newValue == true) ? 1 : 0;

		// Set the value
		PlayerPrefs.SetInt ("Sound", tmp);

	}




}



