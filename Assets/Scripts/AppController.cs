﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour {

	// Singleton
	public static AppController instance;


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

	private bool scoreQualifies = false;
	public bool ScoreQualifies {
		get {
			return scoreQualifies;
		}	
		set {
			scoreQualifies = value;
		}
	}


	// final player score
	private int finalScore = 0;
	public int FinalScore {
		get {
			return finalScore;
		}
		set {
			finalScore = value;
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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// Load Scene Navigation
	public void LoadScene (string scene)
	{

		SceneManager.LoadScene(scene);

	}


	public void PrefsSetSound(bool newValue)
	{
		// convert bool to int
		int tmp = (newValue == true) ? 1 : 0;

		// Set the value
		PlayerPrefs.SetInt ("Sound", tmp);

	}






}



