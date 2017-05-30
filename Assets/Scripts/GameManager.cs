using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Singleton
	//public static GameManager instance;

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


	private bool isNewGame = true;

	// which turn are we on?
	private int currentTurn = 1;

	// how many throws?
	private int maxTurns = 6;

	private int turnDieSelectedCount = 0;


	private bool hasQualifierOne = false;
	private bool hasQualifierFour = false;

	// Green color
	//private Color buttonActiveColor = new Color(67f,184f,38f,1);
	private Color buttonActiveColor = Color.green;

	// Gray color
	private Color buttonDisabledColor = Color.gray;

//	private bool scoreQualifies = false;
//	public bool ScoreQualifies {
//		get {
//			return scoreQualifies;
//		}	
//	}


	private int playerScore = 0;
	public int PlayerScore {
		get {
			return playerScore;
		}
	}


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


//	void Awake ()
//	{
//
//		// Create Singleton
//		if (instance == null) {
//
//			instance = this;
//		} else if (instance != this) {
//
//			Destroy(gameObject);
//
//		}
//
//		DontDestroyOnLoad(instance);
//
//	}
//	

	// Use this for initialization
	void Start ()
	{
		// update the UI
		// >> reset score to zero
		// >> reset qualifiers to NO
		// >> reset turn counter to 1
//		if (isNewGame) {
//
//			Debug.Log("Update() New Game");

			UpdateUI();

			// Roll the dice!
			StartNewTurn();

//			isNewGame = false;
//
//		}

		GameObject.FindGameObjectWithTag ("ButtonNextRoll").GetComponent<Button> ().image.color = buttonDisabledColor;

														
	}


//	void Update ()
//	{
//
//
//
//
//	}


	void StartNewTurn ()
	{

		// Start of turn
		Debug.Log ("Turn " + currentTurn + "; DiceRemaining: " + RemainingDiceCount + "; Q1: " + hasQualifierOne + "; Q4: " + hasQualifierFour + "; Score: " + playerScore); 


		RollDice();

	}



	// dice rolling
	public void RollDice ()
	{

		UpdateUI();

		//Debug.Log("RollDice() called.  Rolling " + RemainingDiceCount);

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

			turnDieSelectedCount++;

//			if (gameObj.name == null && gameObj == null) {
//				Debug.Log("INVALID ENTRY");
//
//			}

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

		// Did they at least select one die
		if (turnDieSelectedCount > 0) {
			GameObject.FindGameObjectWithTag ("ButtonNextRoll").GetComponent<Button> ().image.color = buttonActiveColor;

			Debug.Log("selected die count > 0, changing color to " + buttonActiveColor);
		} else {
			GameObject.FindGameObjectWithTag ("ButtonNextRoll").GetComponent<Button> ().image.color = buttonDisabledColor;

			Debug.Log("selected die count == 0, changing color to Color.gray");
		}
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

		} 

		// no dice left, end the game
		if (RemainingDiceCount <= 0) {

			isGameOver = true;

		}


		// finish turn
		EndTurnUpdate();


	}


	// Wrap up the turn
	public void EndTurnUpdate ()
	{


		// increment to next turn
		currentTurn++;

		// Update the UI
		UpdateUI ();

		// check if game is over
		if (currentTurn > maxTurns) {

			isGameOver = true;

		} 


		//Debug.Log("EndTurnUpdate() Called.  Now turn: " + currentTurn + "; Dice Remaining: " + RemainingDiceCount + "; GameOver Status: " + isGameOver);


		// is the game over for any reason?
		if (isGameOver == true) {

			EndGame();

		} else {

			// game not over, start next round
			StartNewTurn ();

		}

	}


	public void UpdateUI ()
	{

		//Debug.Log("UpdateUI() Called");

		// update Turn Counter
		turnNumberValue.GetComponent<Text> ().text = currentTurn.ToString ();

		// Update player score
		scoreNumberValue.GetComponent<Text> ().text = playerScore.ToString ();

		// Update for qualifier on 1
		//Debug.Log("hasQualifierOne: " + hasQualifierOne);
		if (hasQualifierOne == true) {
			qualifierOneValue.GetComponent<Text> ().text = "Yes"; 
		} else {
			qualifierOneValue.GetComponent<Text> ().text = "No";
		}

		// Update for qualifier on 4
		qualifierFourValue.GetComponent<Text> ().text = (hasQualifierFour == true) ? "Yes" : "No";


		// does their score qualify
		AppController.instance.ScoreQualifies = (hasQualifierOne == true && hasQualifierFour == true) ? true : false;

	}

	// the Game is Over
	public void EndGame() {

		
		Debug.Log ("EndGame() Called");

		AppController.instance.FinalScore = playerScore;

		AppController.instance.LoadScene("GameOver");


	}



	public void ResetGameVars ()
	{

		Debug.Log ("ResetGameVars() Called");

		// which turn are we on?
		currentTurn = 1;
		hasQualifierOne = false;
		hasQualifierFour = false;
		AppController.instance.ScoreQualifies = false;
		playerScore = 0;
		remainingDiceCount = 6;

		// major reset indicator
		isNewGame = true;

	}



}
