using System;
using System.Runtime.InteropServices;

public class PZSpeedPluginIOS : IPZSpeedPlugin
{
	public void Init(string appId, string apiKey)
	{
		_PZSpeed_Unity_Init(appId, apiKey);
	}

	public void UsePZ(bool usePZ)
	{
		_PZSpeed_Unity_UsePZ(usePZ);
	}

	public void MeasureAppLaunchDelay(bool shouldMeasure)
	{
		_PZSpeed_Unity_MeasureAppLaunchDelay(shouldMeasure);
	}

	public bool IsMeasuringAppLaunchDelay()
	{
		return _PZSpeed_Unity_IsMeasuringAppLaunchDelay();
	}

	public bool AppLaunchCompleted()
	{
		return _PZSpeed_Unity_AppLaunchCompleted();
	}

	public void SetCustomID(string userId)
	{
		_PZSpeed_Unity_SetCustomID(userId);
	}
	
	public string GetPZUUID()
	{
		return _PZSpeed_Unity_GetPZUUID();
	}
	
	public void SetProxy(string proxyIP, int proxyPort)
	{
		_PZSpeed_Unity_SetProxy(proxyIP, proxyPort);
	}

	public void SetLogLevel(PZLogLevel logLevel)
	{
		_PZSpeed_Unity_SetLogLevel((int)logLevel);
	}

	public bool ShouldReportAnalytics
	{
		get
		{
			return _PZSpeed_Unity_ShouldReportAnalytics_get();
		}
		set
		{
			_PZSpeed_Unity_ShouldReportAnalytics_set(value);
		}
	}

	[DllImport("__Internal")] private static extern void _PZSpeed_Unity_Init(string appId, string apiKey);
	[DllImport("__Internal")] private static extern void _PZSpeed_Unity_UsePZ(bool value);

	[DllImport("__Internal")] private static extern void _PZSpeed_Unity_MeasureAppLaunchDelay(bool shouldMeasure);
	[DllImport("__Internal")] private static extern bool _PZSpeed_Unity_IsMeasuringAppLaunchDelay();
	[DllImport("__Internal")] private static extern bool _PZSpeed_Unity_AppLaunchCompleted();

	[DllImport("__Internal")] private static extern void _PZSpeed_Unity_SetLogLevel(int value);

	[DllImport("__Internal")] private static extern void _PZSpeed_Unity_SetCustomID(string userId);
	[DllImport("__Internal")] private static extern string _PZSpeed_Unity_GetPZUUID();
	[DllImport("__Internal")] private static extern void _PZSpeed_Unity_SetProxy(string proxyIP, int proxyPort);
	
	[DllImport("__Internal")] private static extern bool _PZSpeed_Unity_ShouldReportAnalytics_get();
	[DllImport("__Internal")] private static extern void _PZSpeed_Unity_ShouldReportAnalytics_set(bool value);
}
