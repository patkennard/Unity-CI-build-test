using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using XcodeEditorPZ;


public static class PacketZoomXcodePostProcess
{
	private const string iOSFilesPath = "Assets/Plugins/PZSpeed";

	[PostProcessBuild(500)]  //int.MaxValue
	public static void YerdyPostProcessBuild(BuildTarget target, string path) 
	{
		if (target != BuildTarget.iOS)
			return;

		XCProject project = new XCProject(path);

		// add files
		var group = project.GetGroup("PZSpeed");
		var allFiles = Directory.GetFiles(iOSFilesPath, "*.*", SearchOption.TopDirectoryOnly);

		var sourceFiles = allFiles.Where(f => f.EndsWith(".h") || f.EndsWith(".mm") || f.EndsWith(".m"));
		foreach (var f in sourceFiles) {
			project.AddFile(f.Substring("Assets/".Length), group);
		}

		// add PZSpeed.framework
		var results = project.AddFile(Path.Combine(iOSFilesPath, "PZSpeed.framework").Substring("Assets/".Length), group);
		if (results != null) {
			PBXFileReference reference = results.Values.First() as PBXFileReference;
			var frameworkPath = reference.data["path"] as string;
			var frameworkFolder = Path.GetDirectoryName(frameworkPath);
			frameworkFolder = frameworkFolder.Trim('"');
			project.AddFrameworkSearchPaths("\"\\\"$(SRCROOT)/" + frameworkFolder + "\\\"\"");  // hack to get balanced " in the outputted xcode project
		}

		// add CoreTelephony.framework
		{
			PBXGroup frameworkGroup = project.GetGroup( "Frameworks" );
			var frameworkPath = Path.Combine("System/Library/Frameworks", "CoreTelephony.framework");
			project.AddFile(frameworkPath, frameworkGroup, "SDKROOT", true, false);
		}

		// -ObjC linker flag
		project.AddOtherLDFlags("\"-ObjC\"");
		// link libc++.dylib
		project.AddOtherLDFlags("\"-lc++\"");
		// link libz.dylib
		project.AddOtherLDFlags("\"-lz\"");

		FlattenFrameworks(project);

		project.Save();
	}

	// flattens framework search paths into the target level, to fix some incompatibilities with other plugins
	private static void FlattenFrameworks(XCProject project)
	{
		var allFrameworkSearchPaths = new HashSet<string>();

		// collect & remove search paths from the project level
		{
			var projectBuildConfigListId = project.project.data["buildConfigurationList"] as string;
			var buildConfigurations = GetBuildConfigurations(project, projectBuildConfigListId);

			foreach (var xcBuildConfig in buildConfigurations) {
				// collect all unique search paths
				if (xcBuildConfig.buildSettings.ContainsKey("FRAMEWORK_SEARCH_PATHS")) {
					var frameworkSearchPaths = xcBuildConfig.buildSettings["FRAMEWORK_SEARCH_PATHS"];
					foreach (var searchPath in SafeToStringList(frameworkSearchPaths))
						allFrameworkSearchPaths.Add(searchPath);

					// remove from project level, all items will be moved to target level
					xcBuildConfig.buildSettings.Remove("FRAMEWORK_SEARCH_PATHS");
				}
			}
		}

		// collect framework search paths from target level
		foreach (var target in project.nativeTargets.Values) {
			var configurationListId = target.data["buildConfigurationList"] as string;
			var buildConfigurations = GetBuildConfigurations(project, configurationListId);

			foreach (var xcBuildConfig in buildConfigurations) {
				// collect all unique search paths
				if (xcBuildConfig.buildSettings.ContainsKey("FRAMEWORK_SEARCH_PATHS")) {
					var frameworkSearchPaths = xcBuildConfig.buildSettings["FRAMEWORK_SEARCH_PATHS"];
					foreach (var searchPath in SafeToStringList(frameworkSearchPaths))
						allFrameworkSearchPaths.Add(searchPath);
					
					// remove, we'll build a new list to add later
					xcBuildConfig.buildSettings.Remove("FRAMEWORK_SEARCH_PATHS");
				}
			}
		}

		// the double quoted ""$(inherited)"" causes all sorts of issues, remove it
		allFrameworkSearchPaths.Remove("\"\\\"$(inherited)\\\"\"");

		// rebuild framework search paths for the target level
		var finalFrameworkSearchPaths = new PBXList();
		foreach (var searchPath in allFrameworkSearchPaths)
			finalFrameworkSearchPaths.Add(searchPath);

		// set FRAMEWORK_SEARCH_PATHS for each native target
		foreach (var target in project.nativeTargets.Values) {
			var configurationListId = target.data["buildConfigurationList"] as string;
			var buildConfigurations = GetBuildConfigurations(project, configurationListId);
			
			foreach (var xcBuildConfig in buildConfigurations) {
				xcBuildConfig.buildSettings.Add("FRAMEWORK_SEARCH_PATHS", finalFrameworkSearchPaths);
			}
		}
	}

	// Because of Xcode's file format, a single item in a list is often represented simply as a
	// string, not a PBXList.  This function ensures that the value is read as a list
	private static List<string> SafeToStringList(object stringOrPBXList)
	{
		var retVal = new List<string>();
		if (stringOrPBXList is PBXList) {
			PBXList pbxList = (PBXList)stringOrPBXList;
			retVal.AddRange( pbxList.Cast<string>() );
		} else if (stringOrPBXList is string) {
			retVal.Add((string)stringOrPBXList);
		}
		return retVal;
	}

	// traverses XCBuildConfigurationList with object ID 'buildConfigurationListId' to get 
	// all the related XCBuildConfigurations
	private static List<XCBuildConfiguration> GetBuildConfigurations(XCProject project, string buildConfigurationListId)
	{
		var buildConfigurations = new List<XCBuildConfiguration>();

		if (project.configurationLists.ContainsKey(buildConfigurationListId)) {
			var configurationList = project.configurationLists[buildConfigurationListId];
			var configurationIds = configurationList.data["buildConfigurations"];
			
			// find actual XCBuildConfigurations for list of IDs
			foreach (string objId in SafeToStringList(configurationIds))
				buildConfigurations.Add(project.buildConfigurations[objId]);
		}

		return buildConfigurations;
	}
}
