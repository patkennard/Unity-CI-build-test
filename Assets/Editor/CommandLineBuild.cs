using System.Collections.Generic;
using UnityEditor;


public class CommandLineBuild
{
	static private string[] collectBuildScenes()
	{
		var scenes = new List<string>();

		foreach (var scene in EditorBuildSettings.scenes)
		{
			if (scene == null)
				continue;
			if (scene.enabled)
				scenes.Add(scene.path);
		}
		return scenes.ToArray();
	}

	[MenuItem(@"Build/BuildIOS")]
	static public void buildIOS()
	{
		var scenes = collectBuildScenes();
		BuildPipeline.BuildPlayer(scenes, "ios", BuildTarget.iOS, BuildOptions.None);
	}
}