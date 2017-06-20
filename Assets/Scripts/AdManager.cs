using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;

public class AdManager : MonoBehaviour {


	public static AdManager instance { set; get; }


	private string bannerId = "ca-app-pub-5683832317492679/6162824547";
	private string videoId = "ca-app-pub-5683832317492679/7639557741";


	void Awake ()
	{

		// Create Singleton
		if (instance == null) {

			instance = this;

		} else if (instance != this) {

			Destroy(gameObject);

		}

		DontDestroyOnLoad(instance);

	}



	// Use this for initialization
	void Start () {



		#if UNITY_EDITOR
		Debug.Log("Unable to display ads from the Editor");
		#elif UNITY_ANDROID
		// initialize Admob ads
		Admob.Instance().initAdmob(bannerId, videoId);

		// preload the video interstitial
		Admob.Instance().loadInterstitial();
		#endif	

	}


	public void ShowBanner ()
	{
		#if UNITY_EDITOR
		Debug.Log("Unable to display Banner ads from the Editor");
		#elif UNITY_ANDROID
		Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.TOP_CENTER, 5);
		#endif	
	}

	public void ShowVideo ()
	{
		#if UNITY_EDITOR
		Debug.Log("Unable to play Video ads from the Editor");
		#elif UNITY_ANDROID
		if (Admob.Instance ().isInterstitialReady()) {

			Admob.Instance().showInterstitial();

		} else {


		}
		#endif	

	}

					
	// Update is called once per frame
	void Update () {
		
	}
}
