using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {


	[SerializeField] private GameObject finalScoreValue;

	// Use this for initialization
	void Start ()
	{

		// update final score

		if (AppController.instance.PlayerScoreQualifies) {

			finalScoreValue.GetComponent<Text> ().text = AppController.instance.PlayerFinalScore.ToString ();

		} else {
			finalScoreValue.GetComponent<Text> ().text = "Score Disqualified";
		}


		// reset to new game
		AppController.instance.IsNewGame = true;

	}


	public void RestartGame ()
	{

		// reload the game scene
		AppController.instance.LoadScene("Game");

	}

}
