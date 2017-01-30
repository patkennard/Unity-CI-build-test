using UnityEngine;
using System.Collections;

public class PZSample : MonoBehaviour {

	private const string PZ_APP_ID = "0de1a119c8fdf675e42036ba86406b8b";
	private const string PZ_API_KEY = "e61ec0afca4e15d2a45bc871b8df7fcae1bbfa05";

	private string url = "http://staging.packetzoom.net/300k.jpg";
	private bool usePZ = true;
	
	private bool loading = false;
	private float lastLoadTime = 0.0f;
	private string loadingDots = "...";
	
	private Texture image;


	void Start()
	{
		// grab the initial value of PZ from the config object
		PacketZoom pz = FindObjectOfType(typeof(PacketZoom)) as PacketZoom;
		if (pz != null)
			usePZ = pz.UsePZ;

		StartCoroutine(UpdateLoadingDots());

		Debug.Log("Is reporting analytics: " + PZSpeed.ShouldReportAnalytics);

		Debug.Log("Is measuring app launch delay: " + PZSpeed.IsMeasuringAppLaunchDelay());
		var success = PZSpeed.AppLaunchCompleted();
		Debug.Log("App launch delay measure success: " + success);
	}

	void OnGUI()
	{
		
		GUI.skin.button.fontSize = (int)(Screen.width/20.0);
		GUI.skin.label.fontSize = (int)(Screen.width/20.0);
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.textField.fontSize = (int)(Screen.width/20.0);
		GUI.skin.textField.alignment = TextAnchor.MiddleLeft;

		// url
		url = GUI.TextField(ScreenRect(0.05f, 0.05f, 0.7f, 0.1f), url);
		
		// Go button/loading animation
		var goRect = ScreenRect(0.8f, 0.05f, 0.15f, 0.1f);
		if (!loading)
		{
			if (GUI.Button(goRect, "GO!"))
			{
				StartCoroutine(DownloadImage());
			}
		}
		else
		{
			GUI.Label(goRect, loadingDots);
		}
		
		// Use PZ toggle
		if (GUI.Button(ScreenRect(0.05f, 0.85f, 0.4f, 0.1f), "Use PZ: " + usePZ))
			TogglePZ();
		
		// load time
		GUI.Label(ScreenRect(0.5f, 0.85f, 0.45f, 0.1f), "Load Time: " + lastLoadTime.ToString("0.00") + "s");
		
		// image
		if (image != null)
		{
			GUI.Label(ScreenRect(0.05f, 0.2f, 0.9f, 0.6f), image);
		}
	}
	
	void TogglePZ()
	{
		usePZ = !usePZ;
		PZSpeed.UsePZ(usePZ);
	}
	
	IEnumerator DownloadImage()
	{
		loading = true;
		float startTime = Time.realtimeSinceStartup;
		image = null;
		
		WWW www = new WWW(url);
		yield return www;
		image = www.texture;
		
		lastLoadTime = Time.realtimeSinceStartup - startTime;
		loading = false;
	}
	
	IEnumerator UpdateLoadingDots()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.2f);
			if (loadingDots.Length >= 3)
				loadingDots = ".";
			else
				loadingDots += ".";
		}
	}
	
	private Rect ScreenRect(float x, float y, float width, float height)
	{
		return new Rect(x * Screen.width, y * Screen.height, width * Screen.width, height * Screen.height);
	}

}
