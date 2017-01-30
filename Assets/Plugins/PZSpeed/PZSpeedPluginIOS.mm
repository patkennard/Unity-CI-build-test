#include <PZSpeed/PZSpeed.h>

#define EXTERN_C extern "C"



static PZSpeedController *pzSpeedController;
static PZSpeedAnalyticsController *pzAnalyticsController;

static PZSpeedController *getSpeedController()
{
	if (!pzSpeedController)
		NSLog(@"You must call PZSpeed.Init(appId, apiKey) first!");
	return pzSpeedController;
}

static PZSpeedAnalyticsController *getAnalyticsController()
{
	if (!pzAnalyticsController)
		NSLog(@"You must call PZSpeed.Init(appId, apiKey) first!");
	return pzAnalyticsController;
}


static NSString *SafeCStringToNSString(const char *str)
{
	if (str == nil) return nil;
	else return [NSString stringWithUTF8String:str];
}

static const char *ReturnNSStringToUnity(NSString *str)
{
	return str != nil ? strdup([str UTF8String]) : NULL;
}


EXTERN_C void _PZSpeed_Unity_Init(const char *appId, const char *apiKey)
{
	pzSpeedController = [PZSpeedController controllerWithAppID:SafeCStringToNSString(appId)
														apiKey:SafeCStringToNSString(apiKey)];
	pzAnalyticsController = [PZSpeedAnalyticsController sharedAnalyticsController];
	pzAnalyticsController.shouldReportAnalytics = YES;
}

EXTERN_C void _PZSpeed_Unity_UsePZ(bool value)
{
	[getSpeedController() usePZ:value];
}

EXTERN_C void _PZSpeed_Unity_MeasureAppLaunchDelay(bool shouldMeasure)
{
	[PZSpeedAnalyticsController measureAppLaunchDelay:shouldMeasure];
}

EXTERN_C bool _PZSpeed_Unity_IsMeasuringAppLaunchDelay()
{
	return [PZSpeedAnalyticsController isMeasuringAppLaunchDelay];
}

EXTERN_C bool _PZSpeed_Unity_AppLaunchCompleted()
{
	return [getAnalyticsController() appLaunchCompleted];
}

EXTERN_C void _PZSpeed_Unity_SetCustomID(const char *userId)
{
	[getSpeedController() setCustomID:SafeCStringToNSString(userId)];
}

EXTERN_C const char *_PZSpeed_Unity_GetPZUUID()
{
	NSString *pzUUID = [[getSpeedController() getPZUUID] description];
	return ReturnNSStringToUnity(pzUUID);
}

EXTERN_C void _PZSpeed_Unity_SetProxy(const char *proxyIP, int proxyPort)
{
	[PZSpeedController setProxyIP:SafeCStringToNSString(proxyIP) andPort:proxyPort];
}

EXTERN_C void _PZSpeed_Unity_SetLogLevel(int value)
{
	[PZSpeedController setPZLogLevel:(PZLogLevel)value];
}

EXTERN_C bool _PZSpeed_Unity_ShouldReportAnalytics_get()
{
	return getAnalyticsController().shouldReportAnalytics;
}

EXTERN_C void _PZSpeed_Unity_ShouldReportAnalytics_set(bool value)
{
	getAnalyticsController().shouldReportAnalytics = value;
}
