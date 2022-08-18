#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class UtilityWindow : EditorWindow
{

	SerializedObject so;


	[MenuItem("Window/THEBADDEST/UtilityWindow")]
	static void Init()
	{
		UtilityWindow window = (UtilityWindow) GetWindow(typeof(UtilityWindow));
		window.Show();
	}


	public Transform[] FindAllChild(Transform transform)
	{
		return transform.GetComponentsInChildren<Transform>();
	}

	public Vector3 FindCenterPoint(Transform[] children)
	{
		if (children.Length == 0)
			return Vector3.zero;
		if (children.Length == 1)
			return children[0].position;
		var bounds = new Bounds(children[0].position, Vector3.zero);
		for (var i = 1; i < children.Length; i++)
			bounds.Encapsulate(children[i].position);
		return bounds.center;
	}

	public Vector3 FindCenterPoint(Transform parent)
	{
		var children = parent.GetComponentsInChildren<Renderer>();
		if (children.Length == 0)
			return Vector3.zero;
		if (children.Length == 1)
			return children[0].bounds.center;
		var bounds = new Bounds(children[0].bounds.center, Vector3.zero);
		for (var i = 1; i < children.Length; i++)
			bounds.Encapsulate(children[i].bounds.center);
		return bounds.center;
	}

	public Vector3 FindCenter(Transform parent)
	{
		var children = parent.GetComponentsInChildren<Renderer>();
		if (children.Length == 0)
			return Vector3.zero;
		if (children.Length == 1)
			return children[0].bounds.center;
		var bounds = children[0].bounds.center;
		for (var i = 1; i < children.Length; i++)
			bounds += (children[i].bounds.center);
		bounds /= children.Length;
		return bounds;
	}

	void OnGUI()
	{
		if (so == null)
		{
			so = new SerializedObject(this);
		}

		ConvertSkinMesh();
		AddComponents();
		CreateAssets();
		AlignAssets();
		SetCenterTransform();
	}


	#region ConvertSkinMesh

	public SkinnedMeshRenderer[] skinnedMeshRenderers;
	bool                         showConvertSkinMesh = false;
	SerializedProperty           skinnedMeshRenderersSerializedProperty;

	void ConvertSkinMesh()
	{
		showConvertSkinMesh = EditorGUILayout.Foldout(showConvertSkinMesh, "Convert Skin Mesh to Mesh Renderer");
		if (skinnedMeshRenderersSerializedProperty == null)
		{
			skinnedMeshRenderersSerializedProperty = so.FindProperty("skinnedMeshRenderers");
		}

		so.Update();
		if (showConvertSkinMesh)
		{
			EditorGUILayout.PropertyField(skinnedMeshRenderersSerializedProperty, true);
			EditorUtility.SetDirty(this);
			so.ApplyModifiedProperties();
			if (GUILayout.Button("Convert"))
			{
				foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
				{
					Mesh       mesh       = renderer.sharedMesh;
					var        materials  = renderer.sharedMaterials;
					GameObject gameObject = renderer.gameObject;
					MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
					meshFilter.sharedMesh = mesh;
					var meshRenderer = gameObject.AddComponent<MeshRenderer>();
					meshRenderer.sharedMaterials = materials;
				}

				for (int i = 0; i < skinnedMeshRenderers.Length; i++)
				{
					DestroyImmediate(skinnedMeshRenderers[i]);
				}

				skinnedMeshRenderers = new SkinnedMeshRenderer[0];
				Debug.Log("Converted Successfully");
			}
		}
	}

	#endregion


	#region AddComponents

	public GameObject[] selectedObjects;
	bool                showAddComponents = false;
	SerializedProperty  selectedObjectsSerializedProperty;

	void AddComponents()
	{
		showAddComponents = EditorGUILayout.Foldout(showAddComponents, "Add Components");
		if (selectedObjectsSerializedProperty == null)
		{
			selectedObjectsSerializedProperty = so.FindProperty("selectedObjects");
		}

		so.Update();
		if (showAddComponents)
		{
			EditorGUILayout.PropertyField(selectedObjectsSerializedProperty, true);
			EditorUtility.SetDirty(this);
			so.ApplyModifiedProperties();
			if (GUILayout.Button("Add"))
			{
				foreach (GameObject selectedObject in selectedObjects)
				{
					selectedObject.AddComponent<BoxCollider>();
				}

				Debug.Log("Added Successfully");
			}
		}
	}

	#endregion


	#region CreateAssets

	public GameObject[] selectedObjectsForCreateAssets;
	bool                showCreateAssets = false;
	SerializedProperty  selectedObjectsForCreateAssetsSerializedProperty;

	void CreateAssets()
	{
		showCreateAssets = EditorGUILayout.Foldout(showCreateAssets, "Create Assets");
		if (selectedObjectsForCreateAssetsSerializedProperty == null)
		{
			selectedObjectsForCreateAssetsSerializedProperty = so.FindProperty("selectedObjectsForCreateAssets");
		}

		so.Update();
		if (showCreateAssets)
		{
			EditorGUILayout.PropertyField(selectedObjectsForCreateAssetsSerializedProperty, true);
			EditorUtility.SetDirty(this);
			so.ApplyModifiedProperties();
			if (GUILayout.Button("Create GameObjects"))
			{
				CreateGameObjectsWithFlow(ref selectedObjectsForCreateAssets);
			}

			if (GUILayout.Button("Create Prefabs"))
			{
				CreatePrefabs(ref selectedObjectsForCreateAssets);
			}

			if (GUILayout.Button("Create Assets"))
			{
				foreach (GameObject selectedObject in selectedObjectsForCreateAssets)
				{
					// var asset = CreateMyAsset<CharacterPart>(selectedObject.name);
					// asset.partName   = selectedObject.name;
					// asset.partPrefab = selectedObject;
				}

				Debug.Log("Created Successfully");
			}
		}
	}

	#endregion


	#region Align to Ground

	public GameObject[] selectedObjectsForAlign;
	bool                showAlignAssets = false;
	SerializedProperty  selectedObjectsForAlignSerializedProperty;

	LayerMask selectedLayer;

	void AlignAssets()
	{
		showAlignAssets = EditorGUILayout.Foldout(showAlignAssets, "Align to Ground");
		if (selectedObjectsForAlignSerializedProperty == null)
		{
			selectedObjectsForAlignSerializedProperty = so.FindProperty("selectedObjectsForAlign");
		}

		so.Update();
		if (showAlignAssets)
		{
			selectedLayer = EditorGUILayout.LayerField("Layer for Objects:", selectedLayer);
			EditorGUILayout.PropertyField(selectedObjectsForAlignSerializedProperty, true);
			EditorUtility.SetDirty(this);
			so.ApplyModifiedProperties();
			if (GUILayout.Button("Align"))
			{
				foreach (GameObject gameObject in selectedObjectsForAlign)
				{
					if (Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, selectedLayer))
					{
						gameObject.transform.rotation = Quaternion.identity;
						gameObject.transform.position = hit.point;
						gameObject.transform.rotation = Quaternion.FromToRotation(gameObject.transform.up, hit.normal);
					}
				}

				Debug.Log("Aligned!");
			}
		}
	}

	#endregion


	Transform parent, target;

	public void SetCenterTransform()
	{
		parent = (Transform) EditorGUILayout.ObjectField("Parent", parent, typeof(Transform), true);
		//	target = (Transform) EditorGUILayout.ObjectField("Parent", target, typeof(Transform), true);
		if (GUILayout.Button("Get Center"))
		{
			GameObject gm  = new GameObject("Target");
			var        pos = FindCenterPoint(parent);
			pos.y                     = 0;
			gm.transform.position     = pos;
			gm.transform.parent       = parent;
			Selection.activeTransform = gm.transform;
			Debug.Log(parent.name + " center created!");
		}
	}


	T CreateMyAsset<T>(string name) where T : ScriptableObject
	{
		var asset = ScriptableObject.CreateInstance<T>();
		AssetDatabase.CreateAsset(asset, $"Assets/{name}.asset");
		AssetDatabase.SaveAssets();
		return asset;
	}

	private Type StringToType(string typeAsString)
	{
		Type typeAsType = Type.GetType(typeAsString);
		return typeAsType;
	}

	public void CreateGameObjectsWithFlow(ref GameObject[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			var currentObj = Instantiate(objects[i].transform.parent.parent.gameObject, objects[i].transform.parent.parent.parent);
			var temp       = currentObj.transform.GetChild(1).Find(objects[i].name);
			var siblings   = new List<GameObject>();
			foreach (Transform t in currentObj.transform.GetChild(1))
			{
				if (objects[i].name != t.name)
				{
					siblings.Add(t.gameObject);
				}
				else
				{
					t.gameObject.SetActive(true);
				}
			}

			foreach (GameObject gameObject in siblings)
			{
				DestroyImmediate(gameObject);
			}

			currentObj.name = objects[i].name;
			objects[i]      = currentObj;
		}
	}

	public void CreatePrefabs(ref GameObject[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			GameObject gameObject = objects[i];
			// Set the path as within the Assets folder,
			// and name it as the GameObject's name with the .Prefab format
			string localPath = "Assets/" + gameObject.name + ".prefab";

			// Make sure the file name is unique, in case an existing Prefab has the same name.
			localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

			// Create the new Prefab.
			objects[i] = PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
		}
	}




}
#endif