using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
#endif


namespace THEBADDEST.Settings
{


	[System.Serializable]
	public class Content
	{

		public string VideoPath => $"C:/Users/Umair Saifullah/Dropbox/Project/{GameSettings.Instance.general.GameName}/Videos/";
		public string VideoName => $"{GameSettings.Instance.general.GameName}_WIP_<Take>";

		#if UNITY_EDITOR


		public Dictionary<string, bool> packages = new Dictionary<string, bool>() {{"com.unity.addressables", false}, {"com.unity.recorder", false}};

		public void UpdatePackages()
		{
			if (File.Exists("Packages/manifest.json"))
			{
				string jsonText = File.ReadAllText("Packages/manifest.json");
				var    listData = packages.ToList();
				foreach (var package in listData)
				{
					if (jsonText.Contains(package.Key))
					{
						UpdatePackageValue(package.Key, true);
					}
				}
			}
		}

		public void UpdatePackageValue(string key, bool value)
		{
			packages[key] = value;
		}


		#endif

	}


}