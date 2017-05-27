using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour {

	// can the die be moved?
	public bool isLocked;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log("OnTriggerEnter2D Detected between this [" + gameObject.name + "] and other [" + other.gameObject.name + "]");

	}


	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Collision2D Detected between this [" + gameObject.name + "] and other [" + other.gameObject.name + "]");

	}
}
