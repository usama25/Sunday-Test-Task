using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif


namespace THEBADDEST.Settings
{


	[System.Serializable]
	public class Build
	{

		public string buildPath;

		public string customBuildAndroidProductName { get; set; }
	
		#if UNITY_EDITOR
		public bool IsMono => PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android) == ScriptingImplementation.Mono2x;
		public string BuildAndroidProductName
		{
			get { return $"{PlayerSettings.productName}_v{GameSettings.Instance.general.BuildVersion}_{GameSettings.Instance.general.BuildNumber}_{PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android)}_{PlayerSettings.Android.targetArchitectures.ToString().Replace(",", "_")}_{(buildOptionAndroid.HasFlag(BuildOptions.Development) ? "Dev" : "Prod")}.{(IsAppBundle ? "aab" : "apk")}"; }
		}
		public string BuildAndroidFolderPath
		{
			get { return $"{buildPath}{PlayerSettings.productName}_v{GameSettings.Instance.general.BuildVersion}_{GameSettings.Instance.general.BuildNumber}_Android/"; }
		}

		public string BuildAndroidFullPathIncludingFolder
		{
			get { return $"{BuildAndroidFolderPath}{(string.IsNullOrEmpty(customBuildAndroidProductName) ? BuildAndroidProductName : customBuildAndroidProductName)}"; }
		}

		public BuildOptions buildOptionAndroid = BuildOptions.AutoRunPlayer | BuildOptions.ShowBuiltPlayer;
		#endif
		public                      bool   IsAppBundle                = false;
		[Header("Keystore")] public bool   signed                     = false;
		public                      string keystoreName, keystorePass = "123456", keyaliasName, keyaliasPass = "123456";

		public void DoBuild()
		{
			#if UNITY_EDITOR
			EditorSceneManager.SaveOpenScenes();
			CreateFolder();
			EditorUserBuildSettings.buildAppBundle               = IsAppBundle;
			EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
			EditorUserBuildSettings.installInBuildFolder         = true;
			EditorUserBuildSettings.SetBuildLocation(BuildTarget.Android, buildPath);
			BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
			buildPlayerOptions.scenes                = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
			buildPlayerOptions.locationPathName      = BuildAndroidFullPathIncludingFolder;
			buildPlayerOptions.target                = BuildTarget.Android;
			buildPlayerOptions.options               = BuildOptions.CompressWithLz4HC | buildOptionAndroid;
			PlayerSettings.Android.useCustomKeystore = signed;
			if (signed)
			{
				PlayerSettings.Android.keystoreName = keystoreName;
				PlayerSettings.Android.keystorePass = keystorePass;
				PlayerSettings.Android.keyaliasName = keyaliasName;
				PlayerSettings.Android.keyaliasPass = keyaliasPass;
			}

			BuildReport  report  = BuildPipeline.BuildPlayer(buildPlayerOptions);
			BuildSummary summary = report.summary;
			if (summary.result == BuildResult.Succeeded)
			{
				UnityEngine.Debug.Log($"BuildState: Build succeeded: {summary.totalSize} bytes File: {BuildAndroidFullPathIncludingFolder}");
				GameSettings.Instance.general.OnBuildSuccess();
			}

			if (summary.result == BuildResult.Failed)
			{
				UnityEngine.Debug.Log("BuildState: Build failed");
			}

			#endif
		}

		public void CreateFolder()
		{
			if (!Directory.Exists(buildPath))
			{
				Directory.CreateDirectory(buildPath);
			}
		}

		public void ResetPath()
		{
			buildPath = $"C:/Users/Umair Saifullah/Dropbox/Project/{GameSettings.Instance.general.GameName}/Builds/";
		}

	}


}