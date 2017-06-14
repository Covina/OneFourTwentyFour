using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Store the faces of the dice to show the values
	public Sprite[] diceSpriteArray;

	// the prefab rolled dice button
	public GameObject diceButtonPrefab;
	public GameObject diceRowContainer;

	// The "Keep" or "Select Dice" button that submits the player's turn
	[SerializeField] private Button buttonSubmitTurn;


	private GameObject debugTextObject;

	// UI Score Text Object
	[SerializeField] private GameObject playerScoreNumberValue;



	// PLAYER
	// The dice roll results for the Player
	private List<int> playerRollResults = new List<int>();

	// Track our qualifiers
	private bool playerHasQualifierOne = false;
	private bool playerHasQualifierFour = false;

	// store player score during the game
	private int playerScore = 0;



	// Opponent 1
	private GameObject opp1MasterObject;
	private bool cpu1HasQualifierOne = false;
	private bool cpu1HasQualifierFour = false;
	private int cpu1CalculatedScore = 0;
	[SerializeField] private GameObject opp1DiceContainer;
	[SerializeField] private GameObject opp1ScoreNumberValue;
	[SerializeField] private GameObject opp1QualifierOneValue;
	[SerializeField] private GameObject opp1QualifierFourValue;

	// AI Player 1
	private ComputerAIController ai1 = new ComputerAIController();
	List<int> ai1Results = new List<int>();



	// Opponent 2
	private GameObject opp2MasterObject;
	private bool opp2FilledQ1 = false;
	private bool opp2FilledQ4 = false;
	private int opp2CalcScore = 0;
	[SerializeField] private GameObject opp2DiceContainer;
	[SerializeField] private GameObject opp2ScoreNumberValue;
	[SerializeField] private GameObject opp2QualifierOneValue;
	[SerializeField] private GameObject opp2QualifierFourValue;

	// AI Player 2
	private ComputerAIController ai2 = new ComputerAIController();
	List<int> ai2Results = new List<int>();


	// Opponent 3
	private GameObject opp3MasterObject;
	private bool opp3FilledQ1 = false;
	private bool opp3FilledQ4 = false;
	private int opp3CalcScore = 0;
	[SerializeField] private GameObject opp3DiceContainer;
	[SerializeField] private GameObject opp3ScoreNumberValue;
	[SerializeField] private GameObject opp3QualifierOneValue;
	[SerializeField] private GameObject opp3QualifierFourValue;

	// AI Player 3
	private ComputerAIController ai3 = new ComputerAIController();
	List<int> ai3Results = new List<int>();



	// Is the game over?
	private bool isGameOver = false;

	// which turn are we on?
	private int currentTurn = 0;

	// how many throws?
	private int maxTurns = 6;


	//private int playerUsedDice = 0;

	// Active Keep button color
	private Color buttonActiveColor = Color.white;

	// Deactive Keep button color
	private Color buttonDisabledColor = Color.gray;


	// how many dice did the player select on this turn
	private int turnDieSelectedCount = 0;
	public int TurnDieSelectedCount {
		get {
			return turnDieSelectedCount;
		}	
		set {
			turnDieSelectedCount = value;
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




	// Use this for initialization
	private void Start ()
	{

		debugTextObject = GameObject.Find("Debug");
//		debugTextObject.GetComponent<Text>().text = "1";


//		// if its a brand new game, make sure we reset all values
//		if (AppController.instance.IsNewGame) {
//
//			ResetGameVars();
//
//			// game is in session!
//			AppController.instance.IsNewGame = false;
//
//		}


		// Generate Computer Opponent 1
		opp1MasterObject = GameObject.FindWithTag("CPU1");
		ai1.SetDifficulty(0);
		ai1Results = ai1.ReturnResult();
		AppController.instance.IsCPU1ScoreQualified = ai1.IsScoreQualified;
		AppController.instance.CPU1FinalScore = ai1.AIScore;
		//ai1.PrintResults();

		// Generate Computer Opponent 2
		opp2MasterObject = GameObject.FindWithTag("CPU2");
		ai2.SetDifficulty(1);
		ai2Results = ai2.ReturnResult();
		AppController.instance.IsCPU2ScoreQualified = ai2.IsScoreQualified;
		AppController.instance.CPU2FinalScore = ai2.AIScore;
		//ai2.PrintResults();

		// Generate Computer Opponent 3
		opp3MasterObject = GameObject.FindWithTag("CPU3");
		ai3.SetDifficulty(2);
		ai3Results = ai3.ReturnResult();
		AppController.instance.IsCPU3ScoreQualified = ai3.IsScoreQualified;
		AppController.instance.CPU3FinalScore = ai3.AIScore;
		//ai3.PrintResults();


		// Start the turn
		StartNewTurn();


		// Update the UI with Results
		UpdateAllGameUI();
																																																		
	}


	public void StartFirstTurn ()
	{

		// Update our UI
		UpdateAllGameUI();
	}


	// Start the turn
	public void StartNewTurn ()
	{  	//Debug.Log("StartNewTurn() Called");


//		debugTextObject.GetComponent<Text>().text = "3";

		Debug.Log ("Turn " + currentTurn + "; DiceRemaining: " + RemainingDiceCount + "; PQ1: " + playerHasQualifierOne + "; PQ4: " + playerHasQualifierFour + "; Score: " + playerScore); 

		// this turn's selected count
		turnDieSelectedCount = 0;


		// get rid of all previous playing dice if any exist in view
		if (GameObject.FindObjectsOfType<Die> ().Length > 0) {

			foreach (Die obj in GameObject.FindObjectsOfType<Die>()) {

				Destroy (obj.gameObject);

			}

		}


		// Update button toggle - useful for Start Of Turn
		UpdateSubmitButtonDisplay ();


		// Loop through remaining dice and place
		for (int i = currentTurn; i < maxTurns; i++) {

			// generate random value
			int randomResult = Random.Range(0, 6);
			//Debug.Log("Die [" + i + "] result is [" + randomResult + "]");

			GameObject tmp = Instantiate(diceButtonPrefab, diceRowContainer.transform) as GameObject;

			// rename the GameObject
			tmp.name = "DieCastButton_" + i;

			// store the Die Value with the Die
			tmp.GetComponent<Die>().dieValue = randomResult + 1;

			// Update the Die image to show the matching face.
			tmp.GetComponent<Image>().sprite = diceSpriteArray[randomResult];

		}


	}




	// Toggle the Keep or Select Dice button
	public void UpdateSubmitButtonDisplay()
	{
		//debugTextObject.GetComponent<Text>().text = "1a UpSubButtDisp Value [" + turnDieSelectedCount + "]";

		// Did they at least select one die
		if (turnDieSelectedCount > 0) {

			//debugTextObject.GetComponent<Text>().text = "1b UpSubButtDisp Value [" + turnDieSelectedCount + "]";

			// Update text to Keep
			buttonSubmitTurn.GetComponentInChildren<Text>().text = "Keep";

			// Change to Active Color
			buttonSubmitTurn.GetComponent<Button> ().image.color = buttonActiveColor;

			// Enable that it is now clickable
			buttonSubmitTurn.interactable = true;

			//Debug.Log("selected die count > 0, changing color to " + buttonActiveColor);
		} else {

			//debugTextObject.GetComponent<Text>().text = "1c UpdateSubmitButtonDisplay [" + turnDieSelectedCount + "]";

			// update text
			buttonSubmitTurn.GetComponentInChildren<Text>().text = "Select Dice";

			// change to Active Color
			buttonSubmitTurn.image.color = buttonDisabledColor;

			// remove the clicky bit
			buttonSubmitTurn.interactable = false;
			//Debug.Log("selected die count == 0, changing color to Color.gray");
		}


	}


	// Process Turn
	private void SubmitTurn ()
	{


		debugTextObject.GetComponent<Text>().text = "2 - SubmitTurn()";

		// Get the dice on the board
		Die[] gameboard = GameObject.FindObjectsOfType<Die> ();

		// did they highlight at least one?
		if (gameboard.Length > 0) {

			// loop through
			foreach (Die gob in gameboard) {

				// if its highlighted, count it.
				if (gob.isHighlighted) {


					if (gob.dieValue == 1 && playerHasQualifierOne == false) {

						playerHasQualifierOne = true;


					} else if (gob.dieValue == 4 && playerHasQualifierFour == false) {

						playerHasQualifierFour = true;

					} else {

						// Store it and Score it
						playerRollResults.Add(gob.dieValue);

						playerScore += gob.dieValue;

					}

					// Increment the turn counter, one for each die
					currentTurn++;
					Debug.Log("Kept dieVal[" + gob.dieValue + "]; currentTurn[" + currentTurn + "]"); 

				}

			}

		} 

		// Has their Score Qualfied qualified?
		if (playerHasQualifierOne == true && playerHasQualifierFour == true) {
			// score has qualified
			AppController.instance.IsPlayerScoreQualified = true;
		}


		// check if game is over
		if (currentTurn >= maxTurns || isGameOver == true) {

			isGameOver = true;
			EndGame();

		} else {

			UpdateAllGameUI();


			// game not over, start next round
			StartNewTurn ();

		}


	}


	// Update all the visual indicators
	private void UpdateGameUIPlayer ()
	{	//Debug.Log("UpdateGameUIPlayer() Called");

		// Update player score
		playerScoreNumberValue.GetComponent<Text> ().text = playerScore.ToString ();


		// Do they have the One qualifier
		if (playerHasQualifierOne == true) {

			// Update the Sprite from Green to Dice
			GameObject.FindGameObjectWithTag ("PlayerQ1").GetComponent<Image> ().sprite = diceSpriteArray [0];
		}

		// Do they have the Four qualifier
		if (playerHasQualifierFour == true) {

			// Update the Sprite from Green to Dice
			GameObject.FindGameObjectWithTag ("PlayerQ4").GetComponent<Image> ().sprite = diceSpriteArray [3];

		} 

		// Loop through stored Player dice and update the UI

		// Player 1 Scored Dice
		int dieCounter = 1;

		foreach (int dieKeptValue in playerRollResults) {

			string playerScorePositionTag = "PlayerD" + dieCounter;

			// player score cant qualify, end game
			if (dieCounter >= 5 && playerHasQualifierOne == false && playerHasQualifierFour == false) {

				// end game
				EndGame ();

			} else {

				// Update the playing field with the die iamges
				int tmp1 = dieKeptValue - 1;
				GameObject.FindGameObjectWithTag (playerScorePositionTag).GetComponent<Image> ().sprite = diceSpriteArray [tmp1];
				//Debug.Log("Updating playerScorePositionTag [" + playerScorePositionTag + "] with diceSpriteArray[" + tmp1 + "]");

			}

			dieCounter++;

		}


	}





	private void UpdateGameUICPU() 
	{	//Debug.Log("UpdateGameUICPU() Called");
		
		// derive how many CPU results to show
		int cpuDiceToDisplay = currentTurn;

		//Debug.Log ("AI to display dice amount: " + cpuDiceToDisplay);

		// if its more than zero, show CPU results
		if (cpuDiceToDisplay > 0) {

			// CPU OPPONENT #1
			int[] a1array = ai1Results.ToArray ();
			int[] a2array = ai2Results.ToArray ();
			int[] a3array = ai3Results.ToArray ();

			// Which Dice Scoring Position are we filling
			int scoreDicePosition = 1;

			// Reset ONE qualifier
			cpu1HasQualifierOne = false;
			opp2FilledQ1 = false;
			opp3FilledQ1 = false;

			// Reset FOUR qualifier
			cpu1HasQualifierFour = false;
			opp2FilledQ4 = false;
			opp3FilledQ4 = false;

			// Reset CPU Score
			cpu1CalculatedScore = 0;
			opp2CalcScore = 0;
			opp3CalcScore = 0;



			// **************************************
			// Update results for Computer Player 1
			for (int i = 0; i < cpuDiceToDisplay; i++) {

				// convenience:  store the face value of the die
				int diceFaceValue = a1array [i];


				//Debug.Log("CPU Display ForLoop;  Processing iteration i[" + i + "] against displayAIDiceCount [" + cpuDiceToDisplay + "];  Showing CPU Die Face Value [" + diceFaceValue + "];  scoreBoxLookup: " + scoreBoxLookup);

				// Was it a 1 and we havent yet qualified?  Then fill it Qualifier ONE
				if (diceFaceValue == 1 && cpu1HasQualifierOne == false) {

					opp1QualifierOneValue.GetComponent<Image> ().sprite = diceSpriteArray [0];
					cpu1HasQualifierOne = true;

					// end loop iteration
					continue;
				}

				// Was it a 1 and we havent yet qualified?  Then fill it Qualifier Four
				if (diceFaceValue == 4 && cpu1HasQualifierFour == false) {

					// display qualifier Four
					opp1QualifierFourValue.GetComponent<Image> ().sprite = diceSpriteArray [3];
					cpu1HasQualifierFour = true;

					// end loop iteration
					continue;
				}


				//Debug.Log ("Current ScoreBox for Opp1: " + scoreBoxLookup);

				// We can only hold 4 Scored spots, so ignore any updates past that
				if (scoreDicePosition <= 4) {

					// convenience: make the by-name lookup easier
					string scoreBoxLookup = "D" + scoreDicePosition;

					// put it into one of the scoring boxes
					opp1DiceContainer.transform.Find (scoreBoxLookup).GetComponent<Image> ().sprite = diceSpriteArray [diceFaceValue - 1];

					// Add it to the CPU score
					cpu1CalculatedScore += diceFaceValue;

					// Update the display of the CPU Score
					opp1ScoreNumberValue.GetComponent<Text>().text = cpu1CalculatedScore.ToString();

				}

				// move to next scoring box
				scoreDicePosition++;

			}

			// Update results for Computer Player 2

			// **************************************
			// Update results for Computer Player 2

			scoreDicePosition = 1;

			for (int i = 0; i < cpuDiceToDisplay; i++) {

				// convenience:  store the face value of the die

				int diceFaceValue = a2array [i];


				//Debug.Log("CPU Display ForLoop;  Processing iteration i[" + i + "] against displayAIDiceCount [" + cpuDiceToDisplay + "];  Showing CPU Die Face Value [" + diceFaceValue + "];  scoreBoxLookup: " + scoreBoxLookup);

				// Was it a 1 and we havent yet qualified?  Then fill it Qualifier ONE
				if (diceFaceValue == 1 && opp2FilledQ1 == false) {

					opp2QualifierOneValue.GetComponent<Image> ().sprite = diceSpriteArray [0];
					opp2FilledQ1 = true;

					// end loop iteration
					continue;
				}

				// Was it a 1 and we havent yet qualified?  Then fill it Qualifier Four
				if (diceFaceValue == 4 && opp2FilledQ4 == false) {

					// display qualifier Four
					opp2QualifierFourValue.GetComponent<Image> ().sprite = diceSpriteArray [3];
					opp2FilledQ4 = true;

					// end loop iteration
					continue;
				}

				//Debug.Log ("Current ScoreBox for Opp1: " + scoreBoxLookup);

				// We can only hold 4 Scored spots, so ignore any updates past that
				if (scoreDicePosition <= 4) {

					// convenience: make the by-name lookup easier
					string scoreBoxLookup = "D" + scoreDicePosition;

					// put it into one of the scoring boxes
					opp2DiceContainer.transform.Find (scoreBoxLookup).GetComponent<Image> ().sprite = diceSpriteArray [diceFaceValue - 1];

					// Add it to the CPU score
					opp2CalcScore += diceFaceValue;

					// Update the display of the CPU Score
					opp2ScoreNumberValue.GetComponent<Text>().text = opp2CalcScore.ToString();

				}

				// move to next scoring box
				scoreDicePosition++;

			}

			// Update results for Computer Player 3
			// **************************************
			// Update results for Computer Player 2

			scoreDicePosition = 1;
			for (int i = 0; i < cpuDiceToDisplay; i++) {

				// convenience:  store the face value of the die

				int diceFaceValue = a3array [i];

				//Debug.Log("CPU Display ForLoop;  Processing iteration i[" + i + "] against displayAIDiceCount [" + cpuDiceToDisplay + "];  Showing CPU Die Face Value [" + diceFaceValue + "];  scoreBoxLookup: " + scoreBoxLookup);

				// Was it a 1 and we havent yet qualified?  Then fill it Qualifier ONE
				if (diceFaceValue == 1 && opp3FilledQ1 == false) {

					opp3QualifierOneValue.GetComponent<Image> ().sprite = diceSpriteArray [0];
					opp3FilledQ1 = true;

					// end loop iteration
					continue;
				}

				// Was it a 1 and we havent yet qualified?  Then fill it Qualifier Four
				if (diceFaceValue == 4 && opp3FilledQ4 == false) {

					// display qualifier Four
					opp3QualifierFourValue.GetComponent<Image> ().sprite = diceSpriteArray [3];
					opp3FilledQ4 = true;

					// end loop iteration
					continue;
				}

				//Debug.Log ("Current ScoreBox for Opp1: " + scoreBoxLookup);

				// We can only hold 4 Scored spots, so ignore any updates past that
				if (scoreDicePosition <= 4) {

					// convenience: make the by-name lookup easier
					string scoreBoxLookup = "D" + scoreDicePosition;

					// put it into one of the scoring boxes
					opp3DiceContainer.transform.Find (scoreBoxLookup).GetComponent<Image> ().sprite = diceSpriteArray [diceFaceValue - 1];

					// Add it to the CPU score
					opp3CalcScore += diceFaceValue;

					// Update the display of the CPU Score
					opp3ScoreNumberValue.GetComponent<Text>().text = opp3CalcScore.ToString();

				}

				// move to next scoring box
				scoreDicePosition++;

			}


		} // end AI update


	}


	// Update all the UI
	private void UpdateAllGameUI ()
	{	//Debug.Log("UpdateAllGameUI() Called");

		// update player section
		UpdateGameUIPlayer();

		// update CPU section
		UpdateGameUICPU();

		debugTextObject.GetComponent<Text>().text = "6 - UpdateAllGameUI ()";

	}


	// the Game is Over
	public void EndGame() 
	{	//Debug.Log ("EndGame() Called");

		// Store the final score of the player
		AppController.instance.PlayerFinalScore = playerScore;

		// Advance to the Game Over scene
		AppController.instance.LoadScene("GameOver");

	}


//	// Reset all the vars to clear the way for a new game
//	public void ResetGameVars ()
//	{	//Debug.Log ("ResetGameVars() Called");
//
//		// which turn are we on?
//		currentTurn = 1;
//		playerHasQualifierOne = false;
//		playerHasQualifierFour = false;
//		AppController.instance.PlayerScoreQualifies = false;
//		playerScore = 0;
//		remainingDiceCount = 6;
//		turnDieSelectedCount = 0;
//
//		// major reset indicator
//		AppController.instance.IsNewGame = true;
//
//	}



}
