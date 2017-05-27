using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour {

	// can the die be moved?
	public bool isLocked = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("OnTriggerEnter2D Detected between this [" + gameObject.name + "] and other [" + other.gameObject.name + "]");


		// lock into the Q1 position if valid
		if (other.tag == "Q1DropZone") {


			// lock it
			isLocked = true;

		} else if (other.tag == "Q4DropZone") {


			// lock it
			//isLocked = true;

		} else {


		GameManager.instance.SnapBack(gameObject);

		}





	}

}
