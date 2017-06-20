using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {


	[SerializeField] private GameObject resultText;

	[SerializeField] private GameObject finalScoreValue;


	[SerializeField] private GameObject cpu1FinalScore;
	[SerializeField] private GameObject cpu2FinalScore;
	[SerializeField] private GameObject cpu3FinalScore;


	// Use this for initialization
	void Start ()
	{

		// Update Player Score
		if (AppController.instance.IsPlayerScoreQualified == true) {
			finalScoreValue.GetComponent<Text> ().text = AppController.instance.PlayerFinalScore.ToString ();
		} else {
			finalScoreValue.GetComponent<Text> ().text = "Disqualified";
		}

		// Update CPU1
		if (AppController.instance.IsCPU1ScoreQualified == true) {
			cpu1FinalScore.GetComponent<Text> ().text = AppController.instance.CPU1FinalScore.ToString ();
		} else {
			cpu1FinalScore.GetComponent<Text> ().text = "DQ";
			AppController.instance.CPU1FinalScore = 0;
		}

		// Update CPU2
		if (AppController.instance.IsCPU2ScoreQualified == true) {
			cpu2FinalScore.GetComponent<Text> ().text = AppController.instance.CPU2FinalScore.ToString ();
		} else {
			cpu2FinalScore.GetComponent<Text> ().text = "DQ";
			AppController.instance.CPU2FinalScore = 0;
		}

		// Update CPU3
		if (AppController.instance.IsCPU3ScoreQualified == true) {
			cpu3FinalScore.GetComponent<Text> ().text = AppController.instance.CPU3FinalScore.ToString ();
		} else {
			cpu3FinalScore.GetComponent<Text> ().text = "DQ";
			AppController.instance.CPU3FinalScore = 0;
		}


		List<int> allResults = new List<int> ();
		allResults.Add (AppController.instance.CPU1FinalScore);
		allResults.Add (AppController.instance.CPU2FinalScore);
		allResults.Add (AppController.instance.CPU3FinalScore);

		// 0 = loss
		// 1 = tie
		// 2 = win
		int outcome = 0;

		if (AppController.instance.IsPlayerScoreQualified) {

			foreach (int opponentScore in allResults) {

				if (AppController.instance.PlayerFinalScore < opponentScore) {

					outcome = 0;
					break;

				} else if (AppController.instance.PlayerFinalScore == opponentScore) {

					outcome = 1;

				} else if (AppController.instance.PlayerFinalScore > opponentScore) {

					outcome = 2;
				}

			}
		}

		Debug.Log ("Outcome [" + outcome + "]");

		// Get messaging correct
		switch (outcome) {


		case 0:
			resultText.GetComponent<Text> ().text = "Defeat!";
			break;

		case 1:
			resultText.GetComponent<Text> ().text = "Draw!";
			break;

		case 2:
			resultText.GetComponent<Text> ().text = "Victory!";
			break;

		default:
			Debug.Log ("No outcome figured out - PROBLEM!");
			break;
		}


		// reset to new game
		AppController.instance.IsNewGame = true;

	}


	public void RestartGame ()
	{


		// Display an interstitial
		AdManager.instance.ShowVideo();

		// reload the game scene
		AppController.instance.LoadScene("Game");

	}

}
