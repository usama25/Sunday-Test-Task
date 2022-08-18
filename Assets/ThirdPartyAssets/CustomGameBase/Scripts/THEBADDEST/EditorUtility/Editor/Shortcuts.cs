using System;
using System.Linq;
using THEBADDEST.DataBase;
using THEBADDEST.Settings;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


namespace THEBADDEST
{


	public class Shortcuts : MonoBehaviour
    {
    
    	[MenuItem("Tools/THEBADDEST/Game/Open Main Scene %&s")]
    	public static void AlignAssets()
    	{
    		var selectedObjectsForAlign = Selection.gameObjects;
    		foreach (GameObject gameObject in selectedObjectsForAlign)
    		{
    			if (Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
    			{
    				gameObject.transform.rotation = Quaternion.identity;
    				gameObject.transform.position = hit.point;
    				gameObject.transform.rotation = Quaternion.FromToRotation(gameObject.transform.up, hit.normal);
    			}
    		}
    
    		Debug.Log("Aligned!");
    	}
    
    	[MenuItem("Tools/THEBADDEST/Game/Open Main Scene %&s")]
    	public static void OpenMainGameScene()
    	{
    		var scenesGUIDs = AssetDatabase.FindAssets("t:Scene");
    		var scenesPaths = scenesGUIDs.Select(AssetDatabase.GUIDToAssetPath);
    		foreach (string scenesPath in scenesPaths)
    		{
    			if (scenesPath.Contains(GameSettings.Instance.general.GameSceneName))
    			{
    				EditorSceneManager.OpenScene(scenesPath);
    				break;
    			}
    		}
    	}
    
    	[MenuItem("Tools/THEBADDEST/Game/Game Settings %&g")]
    	public static void OpenMainGameSettings()
    	{
    		Selection.activeObject = GameSettings.Instance;
    	}
    
    	[MenuItem("Tools/THEBADDEST/Game/Game Database %&d")]
    	public static void OpenMainGameDatabase()
    	{
    		Selection.activeObject = GameDataBase.Instance;
    	}
    	[MenuItem("Tools/THEBADDEST/Ease/Make Group At Zero With Dedicated Name %#.")]
    	public static void MakeGroupAtZeroWithDedicatedName()
    	{
    		GameObject[] objects         = Selection.gameObjects;
    		string       currentCatagory = "";
    		if (objects.Length > 0)
    		{
    			Array.Sort(objects, (a, b) => String.CompareOrdinal(a.name, b.name));
    			Transform parent = null;
    			for (int i = 0; i < objects.Length; i++)
    			{
    				if (parent == null || string.IsNullOrEmpty(currentCatagory) || !objects[i].name.Contains(currentCatagory))
    				{
    					currentCatagory = objects[i].name.Substring(0, 3);
    					parent          = new GameObject(objects[i].name).transform;
    				}
    
    				objects[i].transform.parent = parent;
    			}
    
    			Debug.Log("Group Created");
    		}
    		else
    		{
    			Debug.Log("No selection found.");
    		}
    	}
    
    	[MenuItem("Tools/THEBADDEST/Ease/MakeParentAtZero %.")]
    	public static void MakeParentAtZero()
    	{
    		GameObject[] objects = Selection.gameObjects;
    		if (objects.Length > 0)
    		{
    			Array.Sort(objects, (a, b) => String.CompareOrdinal(a.name, b.name));
    			Transform parent = new GameObject("Parent").transform;
    			parent.parent = objects[0].transform.parent;
    			for (int i = 0; i < objects.Length; i++)
    			{
    				objects[i].transform.parent = parent;
    			}
    
    			Selection.activeObject = parent;
    			Debug.Log("Parent Created");
    		}
    		else
    		{
    			Debug.Log("No selection found.");
    		}
    	}
    }


}
