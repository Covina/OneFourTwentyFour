using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour {

	// can the die be moved?
	public bool isLocked = false;

	public bool isValidPlacement = false;


	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("OnTriggerEnter2D Detected between this [" + gameObject.name + "] and other [" + other.gameObject.name + "]");


		// lock into the Q1 position if valid
		if (other.tag == "Q1DropZone") {


			// valid drop lcoation, so turn off snap back
			isValidPlacement = true;

			// lock it
			isLocked = true;


		} else if (other.tag == "Q4DropZone") {


			// lock it
			//isLocked = true;

			isValidPlacement = true;

		} else {

			isValidPlacement = false;

			// Dropped into an invalid bad object location
			GameManager.instance.SnapBack(gameObject);

		}


	}

}
