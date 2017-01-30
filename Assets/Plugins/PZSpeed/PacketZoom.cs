using UnityEngine;
using System.Collections;

// Script to init PacketZoom based on settings set in the Unity editor

public class PacketZoom : MonoBehaviour {

	// Basic settings

	public string AppID;
	public string ApiKey;

	public PZLogLevel LogLevel = PZLogLevel.Warning;

	public bool MeasureAppLaunchDelay = false;

	// Advanced settings

	public bool UsePZ = true;
	public bool ShouldReportAnalytics = true;


	private static bool initialized = false;

	void Awake()
	{
		// Guard against accidentally initializing twice if this GameObject
		// is created again (for example, navigating back to the scene it was
		// initially added to)
		if (initialized)
			return;

		PZSpeed.SetLogLevel(LogLevel);

		PZSpeed.Init(AppID, ApiKey);
		PZSpeed.UsePZ(UsePZ);
		PZSpeed.ShouldReportAnalytics = ShouldReportAnalytics;
		PZSpeed.MeasureAppLaunchDelay(MeasureAppLaunchDelay);

		initialized = true;
	}
}
