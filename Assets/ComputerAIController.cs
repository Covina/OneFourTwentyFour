using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerAIController : MonoBehaviour {



	// take in a difficulty
	// generate the game results
	// return the results
	// game manager displays turn by turn


	// DIFFICULTY
	// 0 = EASY
	// 1 = MEDIUM
	// 2 = HARD

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



		// roll 6 dice
		// determine dice kept
		// roll remaining dice, keeping min 1 per turn

		List<int> rolledDieOptions = new List<int> ();


		List<int> AIGameResults = new List<int> ();

		// Game starting - rolling 6 new dice
		rolledDieOptions = RollDice (diceRemaining);

		int keeperDie = 0;



		// decide what to keep

		// EASY
		if (difficulty == 0) {


			
			// Look through dice
			foreach (int dieValue in rolledDieOptions) {

				// track if we already kept this value if it was a qualifying 1 or 4
				bool thisOneKept = false;


				// always keep the first 1 seen if needed to qualify
				if (dieValue == 1 && hasQualifierOne == false) {

					// keep the die
					AIGameResults.Add (dieValue);

					// flip to true
					hasQualifierFour == true;

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
					hasQualifierFour == true;

					// reduce remaining die count
					diceRemaining--;

					thisOneKept = true;

					// decision made on this die, move to next value
					//continue;

				}


				// Always keep the highest non-qualifier die on each turn no matter what
				if (dieValue > keeperDie && thisOneKept == false) {

					// keep the die
					keeperDie = dieValue;
										
				}


			}

			// did we find a good die to keep?
			if (keeperDie > 0) {

				// keep the highest value die
				AIGameResults.Add (keeperDie);

				// reduce remaining die count
				diceRemaining--;

			}



		}










	}


	// Dice roller, returns List of integers
	private List<int> RollDice (int quantity)
	{
		List<int> results;

		// Rolling N new dice.
		for (int i = 0; i < quantity; i++) {

			results.Add( Random.Range(1,7) );

		}

		return results;

	}


}
