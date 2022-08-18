
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#endif
using UnityEngine;


namespace THEBADDEST.Settings
{


	[CreateAssetMenu(menuName = "THEBADDEST/Settings")]
	public class GameSettings : SingletonScriptable<GameSettings>
	{

		public General general;
		public Build   build;
		public Content content;


	

		public void Sync()
		{
			#if UNITY_EDITOR
			general.UpdateQuality();
			PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, general.BundleId);
			PlayerSettings.companyName               = "Leyla";
			PlayerSettings.productName               = general.GameName;
			PlayerSettings.bundleVersion             = general.BuildVersion;
			PlayerSettings.Android.bundleVersionCode = general.BuildNumber;
			PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] {general.GameIcon.texture});
			#endif
		}

	}


}