using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Store the Dice prefabs
	public GameObject[] dicePrefabs;

	// Store the faces of the dice to show the values
	public Sprite[] diceSpriteArray;

	// the prefab rolled dice button
	public GameObject diceButtonPrefab;

	[SerializeField] private GameObject turnNumberValue;
	[SerializeField] private GameObject scoreNumberValue;
	[SerializeField] private GameObject qualifierOneValue;
	[SerializeField] private GameObject qualifierFourValue;


	// Is the game over?
	private bool isGameOver = false;

	// which turn are we on?
	private int currentTurn = 1;

	// how many throws?
	private int maxTurns = 6;

	// how many dice did the player select on this turn
	private int turnDieSelectedCount = 0;

	// Track our qualifiers
	private bool hasQualifierOne = false;
	private bool hasQualifierFour = false;

	// Active color
	private Color buttonActiveColor = Color.white;

	// Disabled color
	private Color buttonDisabledColor = Color.gray;

	// store player score during the game
	private int playerScore = 0;


	// how many dice still remain
	private int remainingDiceCount = 6;
	public int RemainingDiceCount {
		get {
			return remainingDiceCount;
		}	
		set {
			remainingDiceCount = value;
		}
	}

	// Use this for initialization
	void Start ()
	{

		// if its a brand new game, make sure we reset all values
		if (AppController.instance.IsNewGame) {

			ResetGameVars();

			// game is in session!
			AppController.instance.IsNewGame = false;

		}

		// Update the UI
		UpdateGameUI();

		// Roll the dice!
		StartNewTurn();
														
	}


	// Start the turn
	void StartNewTurn ()
	{

		//Debug.Log ("Turn " + currentTurn + "; DiceRemaining: " + RemainingDiceCount + "; Q1: " + hasQualifierOne + "; Q4: " + hasQualifierFour + "; Score: " + playerScore); 

		// this turn's selected count
		turnDieSelectedCount = 0;

		// Update our UI
		UpdateGameUI();

		// get rid of all previous playing dice if any
		if (GameObject.FindObjectsOfType<Die> ().Length > 0) {
			foreach (Die obj in GameObject.FindObjectsOfType<Die>()) {

				Destroy (obj.gameObject);

			}
		}


		// Loop through remaining dice and place
		for (int i = 0; i < RemainingDiceCount; i++) {

			// generate random value
			int randomResult = Random.Range(0, 6);
			//Debug.Log("Die [" + i + "] result is [" + randomResult + "]");

			// create the dice
			GameObject tmp = Instantiate(diceButtonPrefab, GameObject.FindWithTag("Field").transform) as GameObject;

			// rename the gameObjects
			tmp.name = "DieCastButton_" + i;

			// assign die value
			tmp.GetComponent<Die>().dieValue = randomResult + 1;

			// Update the image to show the matching face.
			tmp.GetComponent<Image>().sprite = diceSpriteArray[randomResult];

		}

	}


	// Toggle to keep or re-roll
	public void SelectDie (GameObject gameObj)
	{

		//Debug.Log ("SelectDie() called.");


		// If not highlighted, then...
		if (gameObj.GetComponent<Die> ().isHighlighted == false) {

			// set highlight flag
			gameObj.GetComponent<Die> ().isHighlighted = true;

			// add one to turn die keep counter
			turnDieSelectedCount++;


//			Debug.Log("ADDING:  Sizeof selectedDiceForScoring: ["+ diceDictionary.Count +"]; DIctcount: " + dictcount);

			// turn it yellow.
			gameObj.GetComponent<Image> ().color = Color.yellow;


		} else if (gameObj.GetComponent<Die> ().isHighlighted == true) {

			// remove highlight flag
			gameObj.GetComponent<Die> ().isHighlighted = false;

			// turn it back to white.
			gameObj.GetComponent<Image> ().color = Color.white;

			turnDieSelectedCount--;

//			Debug.Log("REMOVING:  Sizeof selectedDiceForScoring: ["+ diceDictionary.Count +"]; DIctcount: " + dictcount);
		}

		UpdateGameUI ();

	}


	// Process Turn
	public void SubmitTurn ()
	{

//		Debug.Log ("Dictcount = " + dictcount);

		// Get the dice on the board
		Die[] gameboard = GameObject.FindObjectsOfType<Die> ();

		//Debug.Log ("Dice in play this turn: " + gameboard.Length);

		// did they highlight at least one?
		if (gameboard.Length > 0) {

			// loop through
			foreach (Die gob in gameboard) {

				// if its highlighted, count it.
				if (gob.isHighlighted) {


					if (gob.dieValue == 1 && hasQualifierOne == false) {

						//Debug.Log("Found 1 Qualifier");

						hasQualifierOne = true;


					} else if (gob.dieValue == 4 && hasQualifierFour == false) {

						//Debug.Log("Found 4 Qualifier");

						hasQualifierFour = true;

					} else {

						playerScore += gob.dieValue;



					}

					// remove die from pool
					RemainingDiceCount--;

				}

			}

		} 

		// Have they qualified?
		if (hasQualifierOne == true && hasQualifierFour == true) {
			// score has qualified
			AppController.instance.ScoreQualifies = true;
		}

		// no dice left, end the game
		if (RemainingDiceCount <= 0) {

			isGameOver = true;

		}

		// increment to next turn
		currentTurn++;

		// Update the UI
		//UpdateGameUI ();

		// check if game is over
		if (currentTurn > maxTurns || isGameOver == true) {

			isGameOver = true;
			EndGame();

		} else {

			// game not over, start next round
			StartNewTurn ();

		}


	}


	// Update all the visual indicators
	public void UpdateGameUI ()
	{

		//Debug.Log("UpdateUI() Called");

		// update Turn Counter
		//turnNumberValue.GetComponent<Text> ().text = currentTurn.ToString ();

		// Update player score
		scoreNumberValue.GetComponent<Text> ().text = playerScore.ToString ();

		// Update for qualifier on 1
		qualifierOneValue.GetComponent<Text> ().text = (hasQualifierOne == true) ? "Yes" : "No";	

		// Update for qualifier on 4
		qualifierFourValue.GetComponent<Text> ().text = (hasQualifierFour == true) ? "Yes" : "No";

		// Did they at least select one die
		if (turnDieSelectedCount > 0) {

			// update text
			GameObject.FindGameObjectWithTag ("ButtonNextRoll").GetComponentInChildren<Text>().text = "Keep";

			// change to Active Color
			GameObject.FindGameObjectWithTag ("ButtonNextRoll").GetComponent<Button> ().image.color = buttonActiveColor;

			// add the clicky bit
			GameObject.FindGameObjectWithTag ("ButtonNextRoll").GetComponent<Button> ().enabled = true;
			//Debug.Log("selected die count > 0, changing color to " + buttonActiveColor);
		}
		else {

			// update text
			GameObject.FindGameObjectWithTag ("ButtonNextRoll").GetComponentInChildren<Text>().text = "Select Dice";

			// change to Active Color
			GameObject.FindGameObjectWithTag ("ButtonNextRoll").GetComponent<Button> ().image.color = buttonDisabledColor;

			// remove the clicky bit
			GameObject.FindGameObjectWithTag ("ButtonNextRoll").GetComponent<Button> ().enabled = false;
			//Debug.Log("selected die count == 0, changing color to Color.gray");
		}


	}

	// the Game is Over
	public void EndGame() {

		
		//Debug.Log ("EndGame() Called");

		// Store the final score of the player
		AppController.instance.FinalScore = playerScore;

		// Advance to the Game Over scene
		AppController.instance.LoadScene("GameOver");


	}


	// Reset all the vars to clear the way for a new game
	public void ResetGameVars ()
	{

		//Debug.Log ("ResetGameVars() Called");

		// which turn are we on?
		currentTurn = 1;
		hasQualifierOne = false;
		hasQualifierFour = false;
		AppController.instance.ScoreQualifies = false;
		playerScore = 0;
		remainingDiceCount = 6;
		turnDieSelectedCount = 0;

		// major reset indicator
		AppController.instance.IsNewGame = true;

	}



}
