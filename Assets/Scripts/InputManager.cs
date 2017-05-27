using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {


	private bool draggingItem = false;

	private GameObject draggedObject;

	private Vector2 touchOffset;

	// is the object moving
	private bool HasInput {
		get {
			return Input.GetMouseButtonDown (0);
		}
	}


	// is the object moving
	private bool DropInput {
		get {
			return Input.GetMouseButtonUp (0);
		}
	}


	private Vector2 CurrentTouchPosition {
		get {
			Vector2 inputPos;
			inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			return inputPos;
		}
	}


	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update ()
	{

		// The button was clicked and not currently holding something
		if (HasInput && !draggingItem) {

			PickUp ();

		} else if (draggingItem && !DropInput) {

			// We're dragging and not dropping
			Drag ();

		} else if (DropInput) {

			// we've let go of the button
			Drop ();

		}

	}



    void PickUp ()
	{

		var inputPosition = CurrentTouchPosition;

		// raycast to hit the object
		RaycastHit2D[] touches = Physics2D.RaycastAll (inputPosition, inputPosition);

		// did we hit an object
		if (touches.Length > 0) {

			Debug.Log ("Raycast hit!");

			// get first item hit

			var hit = touches [0];

			// was it not null?
			// TODO - change this to check Die tagged object and that it's moveable
			if (hit.transform.tag == "Die" && hit.transform.gameObject.GetComponent<Die>().isLocked == false) {

				// turn flag on
				draggingItem = true;

				// set gameobject to drag
				draggedObject = hit.transform.gameObject;

				// preserve touch offset
				touchOffset = (Vector2)hit.transform.position - inputPosition;

				// make it look 20% bigger
				draggedObject.transform.localScale = new Vector3 (1.2f, 1.2f, 1.2f);


				Debug.Log ("Hit Object: " + draggedObject.name);
			}
		}

	}


	void Drag ()
	{

		var inputPosition = CurrentTouchPosition;

		// move the object
		draggedObject.transform.position = inputPosition + touchOffset;

	}


	void Drop ()
	{
		// turn flag on
		draggingItem = false;

		// return the object to its size
        draggedObject.transform.localScale = new Vector3(1f,1f,1f);
	}



}
