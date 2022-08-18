using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;


namespace THEBADDEST.DataBase
{


	[Serializable]
	public class KeyValue<T, T2>
	{

		public T  key;
		public T2 value;

	}

	[Serializable]
	public class GenericCategory<T> : IEnumerable<T>
	{

		public string  name;
		public List<T> elements = new List<T>();

		public IEnumerator<T> GetEnumerator()
		{
			return elements.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}

	[Serializable]
	public class GameObjectCategory : GenericCategory<GameObject>
	{

	}


	#if AddressableAssets


	[Serializable]
	public class AddressableAssetsCategory : GenericCategory<KeyValue<string, UnityEngine.AddressableAssets.AssetReference>>
	{

		public UnityEngine.AddressableAssets.AssetReference GetElement(string key)
		{
			for (int i = 0; i < elements.Count; i++)
			{
				if (elements[i].key == key)
				{
					return elements[i].value;
				}
			}

			return null;
		}

	}

	#endif
	[Serializable]
	public class ObjectCategory : GenericCategory<Object>
	{

	}

	[Serializable]
	public class PoolElement
	{

		public string     nameId;
		public GameObject prefab;
		public int        noOfElement = 1;
		public bool       prebaked    = true;

	}

	[Serializable]
	public class PoolCategory : IEnumerable<PoolElement>
	{

		public List<PoolElement> elements = new List<PoolElement>();

		public IEnumerator<PoolElement> GetEnumerator()
		{
			return elements.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}


	[Serializable]
	public class GridSettings
	{

		public int   width    = 10, length = 10;
		public float cellSize = 10f;

	}


	[Serializable]
	public class ControllerSettings
	{

		public float movementSpeed;

	}

	[CreateAssetMenu(menuName = "THEBADDEST/GameDataBase")]
	public class GameDataBase : SingletonScriptable<GameDataBase>
	{

		public List<GameObjectCategory> m_Categories = new List<GameObjectCategory>();
		#if AddressableAssets
		public List<AddressableAssetsCategory> m_AddressableCategories = new List<AddressableAssetsCategory>();
		#endif
		public List<ObjectCategory> m_AllTypeCategories = new List<ObjectCategory>();
		public PoolCategory         poolCategory        = new PoolCategory();
		public ControllerSettings   controllerSettings;

		#if AddressableAssets
		public UnityEngine.AddressableAssets.AssetReference GetAddressableAsset<T>(string categoryName, int assetIndex)
		{
			foreach (AddressableAssetsCategory category in m_AddressableCategories)
			{
				if (category.name == categoryName)
				{
					for (var i = 0; i < category.elements.Count; i++)
					{
						if (i == assetIndex)
							return category.elements[i].value;
					}
				}
			}

			return default;
		}

		/// <summary>
		/// Hard function its search is very costly use on rare cases
		/// </summary>
		/// <param name="categoryName"></param>
		/// <param name="objectName"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public UnityEngine.AddressableAssets.AssetReference GetAddressableAsset<T>(string categoryName, string objectName)
		{
			foreach (AddressableAssetsCategory category in m_AddressableCategories)
			{
				if (category.name == categoryName)
				{
					return category.GetElement(objectName);
				}
			}

			return default;
		}
		#endif
		public Object GetReference(string categoryName, string objectName)
		{
			foreach (ObjectCategory category in m_AllTypeCategories)
			{
				if (category.name == categoryName)
				{
					for (var i = 0; i < category.elements.Count; i++)
					{
						if (category.elements[i].name == objectName)
						{
							return category.elements[i];
						}
					}
				}
			}

			return null;
		}

		public GameObject InstantiateGameObject(string categoryName, string objectName)
		{
			GameObject gameObject = null;
			foreach (GameObjectCategory category in m_Categories)
			{
				if (category.name == categoryName)
				{
					for (var i = 0; i < category.elements.Count; i++)
					{
						if (category.elements[i].gameObject.name == objectName)
						{
							gameObject = category.elements[i];
							break;
						}
					}
				}
			}

			if (gameObject != null)
				return Object.Instantiate(gameObject);
			return null;
		}

		public GameObject InstantiateGameObject(string categoryName, string objectName, Vector3 position, Quaternion rotation, Transform parent)
		{
			GameObject gameObject = InstantiateGameObject(categoryName, objectName);
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			gameObject.transform.parent   = parent;
			return gameObject;
		}

	}

	#if UNITY_EDITOR
	[CustomEditor(typeof(GameDataBase))]
	public class GameDataBaseEditor : Editor
	{

		GameDataBase dataBase;
		string       categoryName;
		string       categoryType;
		bool         isCreatingCategory = false;

		public override void OnInspectorGUI()
		{
			if (dataBase == null)
			{
				dataBase = (GameDataBase) target;
			}

			base.OnInspectorGUI();
			// if (!isCreatingCategory)
			// {
			// 	if (GUILayout.Button("Create New Category"))
			// 		isCreatingCategory = true;
			// }
			// else
			// {
			// 	categoryName = EditorGUILayout.TextField("Category Name", categoryName);
			// 	categoryType = EditorGUILayout.TextField("Category Type", categoryType);
			// 	if (GUILayout.Button("Add"))
			// 	{
			// 		if (!string.IsNullOrEmpty(categoryType) && Type.GetType(categoryType) != null)
			// 		{
			// 		}
			// 		else
			// 		{
			// 			Debug.Log("it not good");
			// 		}
			// 	}
			//
			// 	if (GUILayout.Button("Cancel"))
			// 		isCreatingCategory = false;
			// }
		}

	}
	#endif


}