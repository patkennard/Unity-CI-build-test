using System;

/// <summary>
/// A "default" no-op IPZSpeedPlugin used when we don't have a platform
/// specific IPZSpeedPlugin
/// </summary>
public class PZSpeedPluginDefault : IPZSpeedPlugin
{
	public void Init(string appId, string apiKey) { }
	public void UsePZ(bool usePZ) { }

	private bool isMeasuringAppLaunchDelay = false;
	public void MeasureAppLaunchDelay(bool shouldMeasure) { isMeasuringAppLaunchDelay = true; }
	public bool IsMeasuringAppLaunchDelay() { return isMeasuringAppLaunchDelay; }
	public bool AppLaunchCompleted() { return isMeasuringAppLaunchDelay; }

	private PZLogLevel currentLogLevel;
	public void SetLogLevel(PZLogLevel logLevel) { currentLogLevel = logLevel; }

	public void SetCustomID(string userId) { }
	public string GetPZUUID() { return string.Empty; }
	public void SetProxy(string proxyIP, int proxyPort) { }

	public bool ShouldReportAnalytics { get; set; }

}