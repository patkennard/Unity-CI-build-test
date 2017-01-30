using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(PacketZoom))]
public class PacketZoomEditor : Editor {
	
	private bool viewAdvancedSettings = false;
	
	public override void OnInspectorGUI()
	{
		PacketZoom pz = (PacketZoom)target;
		
		
		var defaultLabelWidth = LabelWidth;
		LabelWidth = 50.0f;
		
		// App ID / API Key fields
		pz.AppID = EditorGUILayout.TextField("App ID", pz.AppID ?? string.Empty);
		pz.ApiKey = EditorGUILayout.TextField("API Key", pz.ApiKey ?? string.Empty);
		
		
		EditorGUILayout.Separator();
		LabelWidth = 160.0f;
		
		// Log Level field
		pz.LogLevel = (PZLogLevel)EditorGUILayout.EnumPopup("Log Level", pz.LogLevel);
		
		
		EditorGUILayout.Separator();
		
		
		// Measure App Launch Delay
		pz.MeasureAppLaunchDelay = EditorGUILayout.Toggle("Meausure App Launch Delay", pz.MeasureAppLaunchDelay);
		if (pz.MeasureAppLaunchDelay) {
			EditorGUILayout.HelpBox("You must call PZSpeed.AppLaunchCompleted() once your app has finished launching", MessageType.Info);
		}
		
		EditorGUILayout.Separator();
		
		// Advanced Settings
		viewAdvancedSettings = EditorGUILayout.Foldout(viewAdvancedSettings, "Advanced Settings");
		if (viewAdvancedSettings) {
			// Use PZ
			pz.UsePZ = EditorGUILayout.Toggle("Enable PacketZoom", pz.UsePZ);
			
			// Should Report Analytics
			pz.ShouldReportAnalytics = EditorGUILayout.Toggle("Should Report Analytics", pz.ShouldReportAnalytics);
			
			LabelWidth = 140.0f;
		}
		
		LabelWidth = defaultLabelWidth;
	}
	
	
	// Unity 4.2 and lower doesn't support EditorGUIUtility.labelWidth unfortunately, so we wrap it down here based on the version on Unity
	// NOTE: The only difference as a result of this is the Inspector UI is a little uglier
	#if UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0
	private float LabelWidth { get; set; }  // simply no-ops
	#else
	private float LabelWidth {
		get { return EditorGUIUtility.labelWidth; }
		set { EditorGUIUtility.labelWidth = value; }
	}
	#endif
	
}
