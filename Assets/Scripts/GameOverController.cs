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

		if (AppController.instance.ScoreQualifies) {
			finalScoreValue.GetComponent<Text> ().text = AppController.instance.FinalScore.ToString ();
		} else {
			finalScoreValue.GetComponent<Text> ().text = "Score Disqualified";
		}

	}


	public void RestartGame ()
	{

		//GameManager.instance.ResetGameVars();

		AppController.instance.LoadScene("Game");

	}

}
