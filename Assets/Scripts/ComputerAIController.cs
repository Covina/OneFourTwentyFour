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

//		int turnCounter = 1;

		// Loop through the turns until we run out of dice.
		while (diceRemaining > 0) {

			// Rolling Dice
			rolledDieOptions = RollDice (diceRemaining);

			// set to zero
			keeperDie = 0;


			// EASY DIFFICULTY
			// Likely to keep qualifiers when seen, adds random die 
			if (difficulty == 0) {

				// Look through dice
				foreach (int dieValue in rolledDieOptions) {

					// track if we already kept this value if it was a qualifying 1 or 4
					thisOneKept = false;

					// always keep the first 1 seen if needed to qualify
					if (dieValue == 1 && hasQualifierOne == false) {


						// Choose to save a ONE for qualification be a 50/50 shot

						if (Random.Range (0, 2) > 0) {

							// keep the die
							AIGameResults.Add (dieValue);

							// flip to true
							hasQualifierOne = true;

							// reduce remaining die count
							diceRemaining--;

							thisOneKept = true;
						}


					}

					// always keep the first 4 seen if needed to qualify
					if (dieValue == 4 && hasQualifierFour == false) {

						// Choose to save a FOUR for qualification be a 66% shot

						if (Random.Range (0, 3) == 0) {

							// keep the die
							AIGameResults.Add (dieValue);

							// flip to true
							hasQualifierFour = true;

							// reduce remaining die count
							diceRemaining--;

							thisOneKept = true;
						}
					}

				} // end FOR loop


				// Keeps a random dice if qualifier picked.
				if (thisOneKept == false) {

					// get the random die value
					keeperDie = rolledDieOptions [Random.Range (0, rolledDieOptions.Count)];

					// store it
					AIGameResults.Add (keeperDie);
					aiScore += keeperDie;

					// reduce dice
					diceRemaining--;

				}

			}



			// MEDIUM DIFFICULTY
			if (difficulty == 1) {

				// Look through dice
				foreach (int dieValue in rolledDieOptions) {

					// track if we already kept this value if it was a qualifying 1 or 4
					thisOneKept = false;

					// always keep the first 1 seen if needed to qualify
					if (dieValue == 1 && hasQualifierOne == false) {

						// keep the die
						AIGameResults.Add (dieValue);

						// flip to true
						hasQualifierOne = true;

						// reduce remaining die count
						diceRemaining--;

						thisOneKept = true;

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

			}


			// HARD DIFFICULTY
			if (difficulty == 2) {

				int optionsCountOne = 0;
				int optionsCountTwo = 0;
				int optionsCountThree = 0;
				int optionsCountFour = 0;
				int optionsCountFive = 0;
				int optionsCountSix = 0;

				int pickedThisTurn = 0;

				int highValue = 0;

				// Look through dice
				foreach (int dieValue in rolledDieOptions) {

					if (dieValue == 1)
						optionsCountOne++;
					if (dieValue == 2)
						optionsCountTwo++;
					if (dieValue == 3)
						optionsCountThree++;
					if (dieValue == 4)
						optionsCountFour++;
					if (dieValue == 5)
						optionsCountFive++;
					if (dieValue == 6)
						optionsCountSix++;



					if (dieValue > highValue) {
						highValue = dieValue;
					}
					

				}

				// Keep the first 1 seen
				if (optionsCountOne >= 1 && hasQualifierOne == false) {
					AIGameResults.Add (1);
					hasQualifierOne = true;
					optionsCountOne--;
					diceRemaining--;
					pickedThisTurn++;
				}

				// keep the first 4 seen
				if (optionsCountFour >= 1 && hasQualifierFour == false) {
					AIGameResults.Add (4);
					hasQualifierFour = true;
					optionsCountFour--;
					diceRemaining--;
					pickedThisTurn++;
				}

				// keep all sixes when qualifiers are met
				if (hasQualifierOne && hasQualifierFour && optionsCountSix >= 1) {

					int counter = optionsCountSix;

					for (int i = 0; i < counter; i++) {
						AIGameResults.Add (6);
						optionsCountSix--;
						diceRemaining--;
						pickedThisTurn++;

						aiScore += 6;
					}


				} else {
					// keep the highest value;

					if (diceRemaining > 0 && pickedThisTurn == 0) {

						AIGameResults.Add(highValue);
						diceRemaining--;
						pickedThisTurn++;

						aiScore += highValue;

					}

				}



			}


		} // end while loop


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
		Debug.Log ("AIGameResults Values:[" + aiGameResultsString + "]; Qualified [" + isScoreQualified + "];  Score [" + aiScore + "]");

	}

}
