using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {


	// the prefab rolled dice button
	public GameObject diceButtonPrefab;

	// Use this for initialization
	void Start () {

		GameObject tmp = Instantiate(diceButtonPrefab, GameObject.Find("DiceRow").transform) as GameObject;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
