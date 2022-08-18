#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class Catalog
{

	public string   catalogName;
	public string   path;
	public string[] types;

}


public class MovedObject
{

	public Object root;
	public string oldPath;
	public string newPath;

}

public class QuickExportAsset : EditorWindow
{

	string             nameOfFolder = "Game Art";
	public   Object[]  dataObjects;
	readonly Catalog[] catalogs      = new[] {new Catalog() {catalogName = "Scenes", types = new[] {".unity", ".lighting"}}, new Catalog() {catalogName = "Textures", types = new[] {".png", ".tga", ".jpg"}}, new Catalog() {catalogName = "Models", types = new[] {".fbx", ".obj"}}, new Catalog() {catalogName = "Materials", types = new[] {".mat"}}, new Catalog() {catalogName = "Prefabs", types = new[] {".prefab"}}, new Catalog() {catalogName = "Shaders", types = new[] {".shader", ".cginc"}}, new Catalog {catalogName = "Scripts", types = new[] {".cs"}},};
	bool               folderCreated = false;
	List<MovedObject>  movedObjects;
	int                noOfObjects = 1;
	SerializedProperty dataObjectsProperty;
	SerializedObject   so;
	string             mainFolder;
	bool               moveScript = true;

	[MenuItem("Window/THEBADDEST/QuickExportAsset")]
	static void Init()
	{
		QuickExportAsset window = (QuickExportAsset) GetWindow(typeof(QuickExportAsset));
		window.Show();
	}


	void OnGUI()
	{
		if (so == null)
		{
			so                  = new SerializedObject(this);
			dataObjectsProperty = so.FindProperty("dataObjects");
		}

		EditorGUILayout.HelpBox("Developed By Umair Saifullah", MessageType.Info, true);
		nameOfFolder = EditorGUILayout.TextField("Package Name", nameOfFolder);
		CreateFolders();
		PopulateDataFields();
		CheckDependencies();
		if (dataObjects != null && dataObjects.Length > 0 && Selection.objects.Length > 0)
		{
			OrganiseObjects();
			ExportFolders();
			Revert();
		}
	}

	void ExportFolders()
	{
		if (GUILayout.Button("Export Folders"))
		{
			AssetDatabase.ExportPackage(mainFolder, $"{nameOfFolder}.unitypackage", ExportPackageOptions.Recurse);
		}
	}

	void OrganiseObjects()
	{
		if (GUILayout.Button("Organise"))
		{
			movedObjects = new List<MovedObject>();
			foreach (Object o in Selection.objects)
			{
				MovedObject movedObject = new MovedObject {root = o};
				movedObjects.Add(movedObject);
				ChangeFolder(movedObject);
			}

			for (int i = 0; i < dataObjects.Length; i++)
			{
				MovedObject o1 = new MovedObject {root = dataObjects[i]};
				movedObjects.Add(o1);
				ChangeFolder(o1);
			}

			AssetDatabase.Refresh();
			Debug.Log("Organised!");
		}
	}

	void CheckDependencies()
	{
		if (dataObjects != null && dataObjects.Length > 0)
		{
			if (GUILayout.Button("Check Dependencies"))
				Selection.objects = EditorUtility.CollectDependencies(dataObjects);
		}
	}

	void PopulateDataFields()
	{
		if (folderCreated)
		{
			so.Update();
			EditorGUILayout.PropertyField(dataObjectsProperty, true);
			EditorUtility.SetDirty(this);
			moveScript = EditorGUILayout.Toggle("Move Scripts", moveScript);
		}

		so.ApplyModifiedProperties();
	}

	void CreateFolders()
	{
		if (GUILayout.Button("Create Folders"))
		{
			mainFolder = $"Assets/{nameOfFolder}";
			string[] directories = nameOfFolder.Split(Path.DirectorySeparatorChar);
			string   path        = "Assets";
			for (int i = 0; i < directories.Length; i++)
			{
				string directory = directories[i];
				Debug.Log(directory);
				if (!AssetDatabase.IsValidFolder(Path.Combine(path, directory)))
				{
					string guid = AssetDatabase.CreateFolder(path, directory);
				}

				path = Path.Combine(path, directory);
			}

			foreach (Catalog catalog in catalogs)
			{
				catalog.path = $"{mainFolder}/{catalog.catalogName}";
				if (!AssetDatabase.IsValidFolder(catalog.path))
				{
					string guid          = AssetDatabase.CreateFolder($"{mainFolder}", catalog.catalogName);
					string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
					catalog.path = newFolderPath;
				}
			}

			folderCreated = true;
			Debug.Log("Folder Created Successfully");
		}
	}


	bool IsValidPath(string path)
	{
		return !(path.Contains("Plugins") || path.Contains("Resources") || path.Contains(nameOfFolder) || (!moveScript && GetLog(path) != null && GetLog(path).catalogName == "Scripts"));
	}


	void ChangeFolder(MovedObject o)
	{
		var oldpath = AssetDatabase.GetAssetPath(o.root);
		if (IsValidPath(oldpath))
		{
			o.oldPath = oldpath;
			string fileName      = Path.GetFileName(oldpath);
			var    log           = GetLog(fileName);
			string newFolderPath = $"Assets/{nameOfFolder}";
			if (log != null)
			{
				newFolderPath = log.path;
			}

			o.newPath = $"{newFolderPath}/{fileName}";
			AssetDatabase.MoveAsset(oldpath, o.newPath);
		}
		else
		{
			o.newPath = oldpath;
		}
	}

	void Revert()
	{
		if (GUILayout.Button("Revert"))
		{
			foreach (MovedObject movedObject in movedObjects)
			{
				AssetDatabase.MoveAsset(movedObject.newPath, movedObject.oldPath);
			}

			AssetDatabase.Refresh();
			Debug.Log("Reverted");
		}
	}

	Catalog GetLog(string fileName)
	{
		fileName = fileName.ToLower();
		foreach (Catalog catalog in catalogs)
		{
			foreach (string type in catalog.types)
			{
				if (fileName.Contains(type))
				{
					return catalog;
				}
			}
		}

		return null;
	}

}
#endif