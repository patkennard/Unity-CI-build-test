using UnityEditor;
using UnityEngine;
using System.Collections;

public class PacketZoomSetupWindow : EditorWindow
{
	private const float Width = 400.0f;
	private const float Height = 200.0f;
	private const float Padding = 10.0f;

	private const string WelcomeText = 
			"Welcome to PacketZoom setup!  Please ensure you are in your game's initial load scene.\n\n" +
			"If you haven't already, register your app in the PacketZoom dashboard.\n";

	private const string WebpageURL = "http://packetzoom.com";

	[MenuItem("PacketZoom/Setup...")]
	public static void SetupMenuItem()
	{
		var window = new PacketZoomSetupWindow();
		var existing = window.FindExistingPacketZoomObject();
		if (existing != null) {
			EditorUtility.DisplayDialog("PacketZoom", "PacketZoom has already been setup, you change any settings on the 'PacketZoom' GameObject", "OK");
			Selection.activeGameObject = existing;
			return;
		}

		window.title = "PacketZoom";
		window.minSize = window.maxSize = new Vector2(Width, Height);
		window.ShowUtility();
	}

	[MenuItem("PacketZoom/Open Website...")]
	public static void OpenWebsiteMenuItem()
	{
		OpenWebsite();
	}

	private static void OpenWebsite()
	{
		Application.OpenURL(WebpageURL);
	}

	private string appId = string.Empty;
	private string apiKey = string.Empty;

	void OnGUI()
	{
		GUI.skin.label.wordWrap = true;

		GUILayout.BeginVertical();
		{
			GUILayout.Label(WelcomeText);

			if (GUILayout.Button("PacketZoom Website", GUILayout.Width(Width * 0.7f))) {
				OpenWebsite();
			}
			GUILayout.Space(10.0f);

			GUI.skin.label.fontStyle = FontStyle.Bold;
			GUILayout.Label("App Settings");
			GUI.skin.label.fontStyle = FontStyle.Normal;

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("App ID", GUILayout.Width(50.0f));
				appId = GUILayout.TextField(appId);
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("API Key", GUILayout.Width(50.0f));
				apiKey = GUILayout.TextField(apiKey);
			}
			GUILayout.EndHorizontal();

			if (GUILayout.Button("Setup")) {
				ClickedSetupButton();
			}
		}
		GUILayout.EndVertical();
	}

	private GameObject FindExistingPacketZoomObject()
	{
		var packetZoom = GameObject.FindObjectOfType(typeof(PacketZoom)) as PacketZoom;
		if (packetZoom != null)
			return packetZoom.gameObject;
		else
			return null;
	}

	private void ClickedSetupButton()
	{
		if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(apiKey)) {
			EditorUtility.DisplayDialog("PacketZoom", "Please make sure you have entered your app ID and API key", "OK");
			return;
		}

		// possibly update existing object if something really went wrong
		var existing = FindExistingPacketZoomObject();
		if (existing != null) {
			var packetZoom = existing.GetComponent<PacketZoom>();
			packetZoom.AppID = appId;
			packetZoom.ApiKey = apiKey;
			Selection.activeGameObject = existing;
			Close();
			return;
		}

		Selection.activeGameObject = AddPacketZoomGameObject(appId, apiKey);
		Close();
	}

	private GameObject AddPacketZoomGameObject(string appId, string apiKey)
	{
		var go = new GameObject(typeof(PacketZoom).ToString());
		var packetZoom = go.AddComponent<PacketZoom>();
		packetZoom.AppID = appId;
		packetZoom.ApiKey = apiKey;
		return go;
	}

}
