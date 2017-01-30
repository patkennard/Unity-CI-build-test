using UnityEngine;

public enum PZLogLevel
{
	NoLogs = 0,
	Error = 1,
	Warning = 2,
	Info = 3, //Default
	VInfo = 4,
	Debug = 5,
}


public static class PZSpeed
{
	/// <summary>
	/// Initializes PacketZoom
	/// </summary>
	/// <param name="appId">The app ID provided by PacketZoom</param>
	/// <param name="apiKey">The API key provided by PacketZoom</param>
	public static void Init(string appId, string apiKey)
	{
		plugin.Init(appId, apiKey);
	}

	/// <summary>
	/// Starts or stops using PZSpeed for making url requests
	/// </summary>
	/// <param name="usePZ">Boolean value specifying if PZSpeed should be used or not</param>
	public static void UsePZ(bool usePZ)
	{
		plugin.UsePZ(usePZ);
	}

	/// <summary>
	/// Setting to measure app launch time.
	/// NOTE: If you set this to <c>true</c>, you must call AppLaunchCompleted() once
	/// your app has finished launching
	/// </summary>
	/// <param name="shouldMeasure">Whether or not app launch delay should be measured</param>
	public static void MeasureAppLaunchDelay(bool shouldMeasure)
	{
		plugin.MeasureAppLaunchDelay(shouldMeasure);
	}

	/// <summary>
	/// Boolean value that determines if App launch delay is measured by PZSpeed. By Default, this is set to <c>false</c>
	/// </summary>
	/// <returns><c>true</c> if is measuring app launch delay; otherwise, <c>false</c>.</returns>
	public static bool IsMeasuringAppLaunchDelay()
	{
		return plugin.IsMeasuringAppLaunchDelay();
	}

	/// <summary>
	/// This method marks an end of app launch. Failing to call this method when PZSpeed is set to 
	/// measure app launch delay will result in app launch to be marked as never ending.
	/// </summary>
	/// <returns><c>true</c> if success, <c>false</c> if app launch delay is not being measured</returns>
	public static bool AppLaunchCompleted()
	{
		return plugin.AppLaunchCompleted();
	}

	/// <summary>
	/// Sets a custom ID that the PZSpeed will report with the init call and this could also be used to acheive additional user level details and control
	/// </summary>
	/// <param name="userId">userId to set</param>
	public static void SetCustomID(string userId)
	{
		plugin.SetCustomID(userId);
	}

	/// <summary>
	/// Returns the pzUUID.
	/// </summary>
	/// <returns>The pzUUID</returns>
	public static string GetPZUUID()
	{
		return plugin.GetPZUUID();
	}

	/// <summary>
	/// Temporary method to set pz proxy ip and port. This will be removed in release versions
	/// </summary>
	/// <param name="proxyIP">Proxy IP</param>
	/// <param name="proxyPort">Proxy port</param>
	public static void SetProxy(string proxyIP, int proxyPort)
	{
		plugin.SetProxy(proxyIP, proxyPort);
	}
	
	/// <summary>
	/// Sets the logging level to be used by the SDK
	/// </summary>
	/// <param name="logLevel">Log level to set</param>
	public static void SetLogLevel(PZLogLevel logLevel)
	{
		plugin.SetLogLevel(logLevel);
	}

	public static bool ShouldReportAnalytics
	{
		get
		{
			return plugin.ShouldReportAnalytics;
		}
		set
		{
			plugin.ShouldReportAnalytics = value;
		}
	}

	private static IPZSpeedPlugin plugin;

	static PZSpeed()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		plugin = new PZSpeedPluginIOS();
#else
		plugin = new PZSpeedPluginDefault();
#endif
	}
}
