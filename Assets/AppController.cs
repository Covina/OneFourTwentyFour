using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour {

	// Singleton
	public static AppController instance;


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
}
