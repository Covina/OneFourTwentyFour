using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	// Store the Dice prefabs
	public GameObject[] dicePrefabs;

	// has the game begun?
	private bool gameStarted = true;

	// which turn are we on?
	private int currentTurn = 0;

	// store the generated dice roll results
	private List<int> rollResults = new List<int>();

	// store the generated dice roll results for snapbacks
	private List<GameObject> diceStartingPositions = new List<GameObject>();



	private Dictionary<string, Vector2> diceDictionary = new Dictionary<string, Vector2>();


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
			RollDice();
		}
					
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void LoadScene (string scene)
	{
		gameStarted = true;
		SceneManager.LoadScene("Game");


	}


	public void SubmitTurn ()
	{
		Debug.Log("Submitted Turn");

	}




	// dice rolling
	public void RollDice ()
	{

		// how many dice to roll?
		// roll and generate results

		// Clear the starting positions of dice
		diceStartingPositions.Clear();

		// Loop through remaining dice and place
		for (int i = 0; i < RemainingDiceCount; i++) {

			// generate random value
			int randomResult = Random.Range(0, 6);
			//Debug.Log("Die [" + i + "] result is [" + randomResult + "]");

			// create the dice
			GameObject tmp = Instantiate(dicePrefabs[randomResult], GameObject.FindWithTag("Field").transform) as GameObject;
			tmp.name = "DieCast" + i;

			// generate X offset
			float xPos = i - ( (remainingDiceCount/2) - 0.5f);
		
			// place the dice
			tmp.transform.position = new Vector2(xPos,0f);

			// store the position for snap backs
			diceStartingPositions.Add(tmp);

			// add dice name and position
			diceDictionary.Add(tmp.name, tmp.transform.position);


		}


	}


	// Send the die back to its starting location
	public void SnapBack (GameObject dieObject)
	{

		Debug.Log("SnapBack() called for object " + dieObject.name + ". IsLocked[" + dieObject.GetComponent<Die>().isLocked + "], IsValidPlacement [" + dieObject.GetComponent<Die>().isValidPlacement + "]");


		// is the object not locked and eligble to return?
		if (!dieObject.GetComponent<Die>().isLocked && !dieObject.GetComponent<Die>().isValidPlacement) {

			// get the starting position

			Vector2 foundDie;

			if (diceDictionary.TryGetValue (dieObject.name, out foundDie)) {

				//			Debug.Log ("Dictionary lookup successful. Lookup key: [" + dieObject.name + "]");

				//			Debug.Log (dieObject.name + " current pos:  " + dieObject.transform.position);
				//			Debug.Log (dieObject.name + " stored pos:  " + foundDie);

				// set the object back to its start
				dieObject.transform.position = foundDie;

			} else {

				//Debug.Log ("Dictionary lookup failed.  Lookup key: [" + dieObject.name + "]");

			}

		}

	}



}
