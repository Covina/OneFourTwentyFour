using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerAIController {



	// take in a difficulty
	// generate the game results
	// return the results
	// game manager displays turn by turn


	// DIFFICULTY
	// 0 = EASY
	// 1 = MEDIUM
	// 2 = HARD

	// Fresh list of final Game Results
	private List<int> AIGameResults = new List<int> ();


	// Did this score qualify
	private bool isScoreQualified = false;
	public bool IsScoreQualified {
		get {
			return isScoreQualified;
		}
		set {
			isScoreQualified = value;
		}
	}


	// Did this score qualify
	private int aiScore = 0;
	public int AIScore {
		get {
			return aiScore;
		}
		set {
			aiScore = value;
		}
	}


	private int difficulty = 0;

	// how many dice are left 
	private int diceRemaining = 6;

	// Track the Qualifier status
	private bool hasQualifierOne = false;

	private bool hasQualifierFour = false;


	// set difficulty settings to adjust decision making
	public void SetDifficulty(int difficultySetting) 
	{
		// set the difficulty setting
		difficulty = difficultySetting;

	}

	// play the game and return the results as a list of kept dice rolls
	public List<int> ReturnResult ()
	{


		// INITIALIZE VARIABLES ==========================

		// Fresh list of rolledDice
		List<int> rolledDieOptions = new List<int> ();

		// to track extra die to keep
		int keeperDie = 0;

		bool thisOneKept = false;
		// ================================================ 



		// EASY DIFFICULTY
		if (difficulty == 0) {

			int turnCounter = 1;

			// Loop through the turns until we run out of dice.
			while (diceRemaining > 0) {


				//Debug.Log("WHILE LOOP() Top;  Dice Remaining: " + diceRemaining);

				// Rolling Dice
				rolledDieOptions = RollDice (diceRemaining);

				// set to zero
				keeperDie = 0;


				// Look through dice
				foreach (int dieValue in rolledDieOptions) {

					// RESET VARIABLES =============================

					// track if we already kept this value if it was a qualifying 1 or 4
					thisOneKept = false;

					// =============================================


					// always keep the first 1 seen if needed to qualify
					if (dieValue == 1 && hasQualifierOne == false) {

						// keep the die
						AIGameResults.Add (dieValue);

						// flip to true
						hasQualifierOne = true;

						// reduce remaining die count
						diceRemaining--;

						thisOneKept = true;

						// decision made on this die, move to next value
						//continue;

					}

					// always keep the first 4 seen if needed to qualify
					if (dieValue == 4 && hasQualifierFour == false) {

						// keep the die
						AIGameResults.Add (dieValue);

						// flip to true
						hasQualifierFour = true;

						// reduce remaining die count
						diceRemaining--;

						thisOneKept = true;

						// decision made on this die, move to next value
						//continue;

					}


					// Always keep the highest non-qualifier die on each turn no matter what
					if (dieValue > keeperDie && thisOneKept == false) {

						// keep the die
						//Debug.Log("Comparing dieValue[" + dieValue + "] to Keeper[" + keeperDie + "]");
						keeperDie = dieValue;
						//Debug.Log("Keeper die now value: " + keeperDie);
					}


				} // end FOR loop


				// did we find a good die to keep?
				if (keeperDie > 0) {

					// keep the highest value die
					AIGameResults.Add (keeperDie);

					aiScore += keeperDie;

					// reduce remaining die count
					diceRemaining--;

				}


				// roleld Dice string
				string rdstr = "";
				foreach (int val in rolledDieOptions) {
					rdstr = rdstr + ", " + val;
				}

				// kept dice string
				string aigr = "";
				foreach (int val in AIGameResults) {
					aigr = aigr + ", " + val;
				}

				//Debug.Log ("Turn[" + turnCounter + "] Complete;  Rolled Dice: [" + rdstr + "]; All Kept Dice [" + aigr + "]");

				// incremenet to next turn
				turnCounter++;

			} // end while loop


		}

		// does the score qualify?
		if (hasQualifierOne && hasQualifierFour) {

			// set that its qualified
			isScoreQualified = true;
		}


		// return the list of results
		return AIGameResults;

	}


	// Dice roller, returns List of integers
	private List<int> RollDice (int quantity)
	{
		List<int> results = new List<int>();

		// Rolling N new dice.
		for (int i = 0; i < quantity; i++) {

			results.Add( Random.Range(1,7) );

		}

		return results;

	}



	public void PrintResults()
	{
		string aiGameResultsString = "";
		foreach (int val in AIGameResults) {
			aiGameResultsString += val + ", ";
		}
		Debug.Log ("AIGameResults Values: " + aiGameResultsString + ";  Qualified?[" + isScoreQualified + "];  Score [" + aiScore + "]");

	}

}
