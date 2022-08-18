using System.Collections.Generic;
using THEBADDEST.DataBase;
using UnityEngine;


namespace THEBADDEST
{


	public class MasterObjectPooler : SingletonLocal<MasterObjectPooler>
	{

		PoolCategory                          poolCategory => GameDataBase.Instance.poolCategory;
		Dictionary<string, Stack<GameObject>> poolDictionary = new Dictionary<string, Stack<GameObject>>();

		Dictionary<string, Transform>            parentsDictionary = new Dictionary<string, Transform>();
		[InspectorButton("Prebake")] public bool canPrebake;

		protected override void Awake()
		{
			base.Awake();
			Init();
		}

		public void Init()
		{
			PoolPrebakeElements();
			foreach (PoolElement element in poolCategory)
			{
				if (!element.prebaked)
				{
					CreateElements(element);
				}
			}
		}

		void PoolPrebakeElements()
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				var p = transform.GetChild(i);
				parentsDictionary.Add(p.name, p);
				var children = p.GetChildrenWithType<Transform>();
				var stack    = new Stack<GameObject>();
				for (int j = 0; j < children.Count; j++)
				{
					stack.Push(children[j].gameObject);
				}

				poolDictionary.Add(p.name, stack);
			}
		}


		public void Prebake()
		{
			foreach (PoolElement element in poolCategory)
			{
				if (element.prebaked)
				{
					CreateElements(element);
				}
			}
		}

		void CreateElements(PoolElement poolElement)
		{
			if (poolElement.noOfElement < 1) return;
			var stack  = new Stack<GameObject>();
			var parent = new GameObject(poolElement.nameId).transform;
			parent.parent = transform;
			parentsDictionary.Add(poolElement.nameId, parent);
			//Fly Weight
			var runtimeObject = Instantiate(poolElement.prefab, parent);
			runtimeObject.SetActive(false);
			runtimeObject.name = $"{poolElement.nameId} {1}";
			stack.Push(runtimeObject);
			for (int i = 0; i < poolElement.noOfElement - 1; i++)
			{
				runtimeObject      = Instantiate(runtimeObject, parent);
				runtimeObject.name = $"{poolElement.nameId} {i + 2}";
				runtimeObject.SetActive(false);
				stack.Push(runtimeObject);
			}

			poolDictionary.Add(poolElement.nameId, stack);
		}

		public GameObject GetNew(string nameId)
		{
			if (poolDictionary.ContainsKey(nameId))
			{
				var newObject = poolDictionary[nameId].Pop();
				newObject.SetActive(true);
				return newObject;
			}

			return null;
		}

		public T GetNew<T>(string nameId)
		{
			if (poolDictionary.ContainsKey(nameId))
			{
				var gObject = poolDictionary[nameId].Pop();
				gObject.SetActive(true);
				return gObject.GetComponent<T>();
			}

			return default(T);
		}

		//
		public T GetNewAt<T>(string nameId, Vector3 position, Quaternion rotation)
		{
			var gObject = GetNew(nameId);
			if (gObject)
			{
				gObject.transform.position = position;
				gObject.transform.rotation = rotation;
				gObject.transform.parent   = null;
				return gObject.GetComponent<T>();
			}

			return default(T);
		}

		//
		public GameObject GetNewAt(string nameId, Vector3 position, Quaternion rotation)
		{
			var gObject = GetNew(nameId);
			if (gObject)
			{
				gObject.transform.position = position;
				gObject.transform.rotation = rotation;
				gObject.transform.parent   = null;
				return gObject;
			}

			return null;
		}

		//
		public void Free(string nameId, GameObject gObject)
		{
			if (poolDictionary.ContainsKey(nameId))
			{
				gObject.SetActive(false);
				gObject.transform.parent = parentsDictionary[nameId];
				poolDictionary[nameId].Push(gObject);
			}
		}

	}


}