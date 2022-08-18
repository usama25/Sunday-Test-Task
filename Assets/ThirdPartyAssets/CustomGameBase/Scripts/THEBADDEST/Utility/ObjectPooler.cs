using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace THEBADDEST
{


	public class ObjectPooler<T> where T : UnityEngine.MonoBehaviour, IPooled<T>
	{

		public T[] instances;

		protected Stack<int> m_FreeIdx;

		public void Initialize(int count, T prefab)
		{
			instances = new T[count];
			m_FreeIdx = new Stack<int>(count);
			var parent = new GameObject($"{prefab.gameObject.name}s").transform;
			for (int i = 0; i < count; ++i)
			{
				instances[i] = Object.Instantiate(prefab);
				instances[i].transform.SetParent(parent);
				instances[i].gameObject.SetActive(false);
				instances[i].poolID = i;
				instances[i].pool   = this;
				m_FreeIdx.Push(i);
			}
		}

		public T GetNew()
		{
			int idx = m_FreeIdx.Pop();
			instances[idx].gameObject.SetActive(true);
			return instances[idx];
		}

		public T GetNewAt(Vector3 position, Quaternion rotation)
		{
			int idx = m_FreeIdx.Pop();
			instances[idx].gameObject.SetActive(true);
			var transform = instances[idx].transform;
			transform.position = position;
			transform.rotation = rotation;
			return instances[idx];
		}

		public void Free(T obj)
		{
			m_FreeIdx.Push(obj.poolID);
			instances[obj.poolID].gameObject.SetActive(false);
		}

	}

	public interface IPooled<T> where T : MonoBehaviour, IPooled<T>
	{

		int             poolID { get; set; }
		ObjectPooler<T> pool   { get; set; }

	}


}