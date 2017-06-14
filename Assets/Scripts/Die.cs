using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Die : MonoBehaviour {

	// can the die be moved?
	public bool isLocked = false;

	public bool isHighlighted = false;

	public int dieValue;

	private GameManager gameManager;


	void Start ()
	{
		// get access to the object
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

	}


	// Toggle to keep or re-roll
	public void SelectDie ()
	{	//Debug.Log ("SelectDie() called.");


		// If not highlighted, then...
		if (isHighlighted == false) {

			// set highlight flag
			isHighlighted = true;

			// turn it yellow.
			GetComponent<Image> ().color = Color.yellow;

			gameManager.TurnDieSelectedCount++;


		} else if (isHighlighted == true) {

			// remove highlight flag
			GetComponent<Die> ().isHighlighted = false;

			// turn it back to white.
			GetComponent<Image> ().color = Color.white;

			gameManager.TurnDieSelectedCount--;

		}

		// Update button
		gameManager.UpdateSubmitButtonDisplay();

	}


}
