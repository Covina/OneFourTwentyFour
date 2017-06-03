using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {


	[SerializeField] private Sprite soundOnImage;
	[SerializeField] private Sprite soundOffImage;
	[SerializeField] private Button soundButtonObject;


	void Start ()
	{

		Debug.Log("OptionsController started, soundValue [" + AppController.instance.IsSoundOn + "]"); 

		// Display the current state
		if (AppController.instance.IsSoundOn == true) {

			soundButtonObject.GetComponent<Image>().sprite = soundOnImage;

		} else {
			soundButtonObject.GetComponent<Image>().sprite = soundOffImage;
		}

	}


	public void ToggleSound ()
	{
		Debug.Log("ToggleSound() Called");
		
		// if its on, set it to off
		if (AppController.instance.IsSoundOn == true) {
			
			soundButtonObject.GetComponent<Image>().sprite = soundOffImage;
			AppController.instance.IsSoundOn = false;

		} else {

			// it was off, so lets turn it on
			soundButtonObject.GetComponent<Image>().sprite = soundOnImage;
			AppController.instance.IsSoundOn = true;
		}



	}


	public void ToggleSoundOn() 
	{
		Debug.Log("ToggleSoundOn() Called");

		// store ON
		AppController.instance.PrefsSetSound(true);

		// Swap to ON Button

		// TODO - enable audio sources



	}


	public void ToggleSoundOff() 
	{
		Debug.Log("ToggleSoundOff() Called");

		// store OFF
		AppController.instance.PrefsSetSound(false);

		// Swap to OFF Button

		// TODO - enable audio sources

	}


}
