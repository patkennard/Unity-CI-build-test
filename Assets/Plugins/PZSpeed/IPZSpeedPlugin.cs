/// <summary>
/// Defines the common interface between Unity and the various
/// native platform code
/// </summary>
public interface IPZSpeedPlugin
{
	void Init(string appId, string apiKey);

	void UsePZ(bool usePZ);

	void MeasureAppLaunchDelay(bool shouldMeasure);
	bool IsMeasuringAppLaunchDelay();
	bool AppLaunchCompleted();

	void SetCustomID(string userId);
	string GetPZUUID();
	void SetProxy(string proxyIP, int proxyPort);

	void SetLogLevel(PZLogLevel logLevel);

	bool ShouldReportAnalytics { get; set; }
}
