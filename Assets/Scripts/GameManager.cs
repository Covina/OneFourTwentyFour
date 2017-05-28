using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// Singleton
	public static GameManager instance;

	// Store the Dice prefabs
	public GameObject[] dicePrefabs;

	// Store the faces of the dice to show the values
	public Sprite[] diceSpriteArray;

	// the prefab rolled dice button
	public GameObject diceButtonPrefab;


	public GameObject turnNumberValue;
	public GameObject scoreNumberValue;
	public GameObject qualifierOneValue;
	public GameObject qualifierFourValue;


	// has the game begun?
	private bool gameStarted = false;

	// which turn are we on?
	private int currentTurn = 1;

	// how many throws?
	private int maxTurns = 4;


	// store the generated dice roll results
	private List<int> rollResults = new List<int>();

	private bool hasQualifierOne = false;
	private bool hasQualifierFour = false;
	private int scoredDiceQuantity = 0;

	private int playerScore = 0;


	// how many dice still remain
	private int remainingDiceCount = 6;

	// Getter for dice remaining
	public int RemainingDiceCount {
		get {
			return remainingDiceCount;
		}	
		set {
			remainingDiceCount = value;
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
	void Start ()
	{
		if (gameStarted) {

			UpdateScore();

			StartNewTurn();
		}

								
	}
	
	// Update is called once per frame
	void Update ()
	{



	}


	// Load Scene Navigation
	public void LoadScene (string scene)
	{

		if(scene == "Game") gameStarted = true;

		SceneManager.LoadScene(scene);

	}



	void StartNewTurn ()
	{

		// Start of turn
		Debug.Log("Turn " + currentTurn + "; DiceRemaining: " + RemainingDiceCount + "; Q1: " + hasQualifierOne + "; Q4: " + hasQualifierFour + "; Score: " + playerScore); 


		// get rid of tall playing dice
		foreach (Die obj in GameObject.FindObjectsOfType<Die>()) {

			Destroy(obj.gameObject);

		}

		// update Turn Counter
		turnNumberValue.GetComponent<Text>().text = currentTurn.ToString();

		RollDice();

	}



	// dice rolling
	public void RollDice ()
	{

		Debug.Log("RollDice() called.  Rolling " + RemainingDiceCount);

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


			if (gameObj.name == null && gameObj == null) {
				Debug.Log("INVALID ENTRY");

			}

//			Debug.Log("ADDING:  Sizeof selectedDiceForScoring: ["+ diceDictionary.Count +"]; DIctcount: " + dictcount);

			// turn it yellow.
			gameObj.GetComponent<Image> ().color = Color.yellow;


		} else if (gameObj.GetComponent<Die> ().isHighlighted == true) {

			// remove highlight flag
			gameObj.GetComponent<Die> ().isHighlighted = false;

			// turn it back to white.
			gameObj.GetComponent<Image> ().color = Color.white;

//			Debug.Log("REMOVING:  Sizeof selectedDiceForScoring: ["+ diceDictionary.Count +"]; DIctcount: " + dictcount);
		}
	}


	// Process Turn
	public void SubmitTurn ()
	{

//		Debug.Log ("Dictcount = " + dictcount);

		// Get Highlighted Die
		Die[] gameboard = GameObject.FindObjectsOfType<Die> ();

		Debug.Log("Dice kept: " + gameboard.Length);

		// did they highlight at least one?
		if (gameboard.Length > 0) {

			// loop through
			foreach (Die gob in gameboard) {

				// if its highlighted, count it.
				if (gob.isHighlighted) {


					if (gob.dieValue == 1 && hasQualifierOne == false) {

						//Debug.Log("Found 1 Qualifier");

						hasQualifierOne = true;

						RemainingDiceCount--;

					} else if (gob.dieValue == 4 && hasQualifierFour == false) {

						//Debug.Log("Found 4 Qualifier");

						hasQualifierFour = true;

						RemainingDiceCount--;

					} else {

						playerScore += gob.dieValue;

						RemainingDiceCount--;

					}

				}

			}


			// update Score
			EndRoundUpdate();

		}


		// look at selected dice
		// first 1 goes to Q
		// first 4 goes to Q
		// any other selected are stored and scored
		// score is updated
		// Turn counter incremented

		// out of turns ends game
		// final result.



	}



	public void EndRoundUpdate ()
	{

		UpdateScore ();

		if (currentTurn >= maxTurns) {

			EndGame ();

		} else {
		
			currentTurn++;

			StartNewTurn ();

		}

	}


	public void UpdateScore ()
	{
		// Update the UI piece
		scoreNumberValue.GetComponent<Text>().text = playerScore.ToString();


		if(hasQualifierOne) qualifierOneValue.GetComponent<Text>().text = "Yes";
		if(hasQualifierFour) qualifierFourValue.GetComponent<Text>().text = "Yes";

	}


	public void EndGame() {

		gameStarted = false;

		LoadScene("GameOver");

	}

}
