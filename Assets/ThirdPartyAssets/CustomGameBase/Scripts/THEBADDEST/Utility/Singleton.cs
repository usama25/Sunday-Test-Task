using UnityEngine;


namespace THEBADDEST
{


	public interface ISingleton<T> where T : Object
	{

		T Instance { get; }

		void SetValue();

	}
	#if AddressableAssets

	/// <summary>
	/// Kindly call Create() before Every Data Creation
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SingletonScriptableAddressable<T> : ScriptableObject where T : Object
	{

		protected static T instance;
		public static T Instance
		{
			get
			{
				Create();
				return instance;
			}
		}

		public static void Create()
		{
			if (instance == null)
			{
				AddressableManager.Get<T>(typeof(T).Name, (result) => { instance = result; });
			}
		}

		public void SetValue()
		{
			name = typeof(T).Name;
		}

	}
	#endif
	public class SingletonScriptable<T> : ScriptableObject where T : Object
	{

		protected static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					var values = Resources.FindObjectsOfTypeAll<T>();
					if (values == null || values.Length == 0)
					{
						values = Resources.LoadAll<T>(string.Empty);
					}

					if (values.Length > 0 && values[0] != null)
					{
						instance = values[0];
						return instance;
					}
				}

				if (instance == null)
				{
					Debug.Log($"Not {nameof(T)} found Kindly add it in Resources");
					#if UNITY_EDITOR
					string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(T)}");
					string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
					return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
					#else
					  return null;
					#endif
				}

				return instance;
			}
		}


		public void SetValue()
		{
			name = typeof(T).Name;
		}

	}

	public abstract class Singleton<T> : MonoBehaviour where T : Component
	{

		/// <summary>
		/// The instance.
		/// </summary>
		protected static T instance;

		protected virtual void OnValidate()
		{
			gameObject.name = typeof(T).Name;
		}

	}


	public abstract class SingletonInternal<T> : Singleton<T> where T : Component
	{

		protected virtual void Awake()
		{
			instance = this as T;
		}

	}

	public abstract class SingletonLocal<T> : Singleton<T> where T : Component
	{

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<T>();
					if (instance == null)
					{
						instance = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
					}
				}

				return instance;
			}
		}

		protected virtual void Awake()
		{
			if (instance == null)
				instance = this as T;
		}

	}

	public abstract class SingletonPersistent<T> : SingletonLocal<T> where T : Component
	{

		protected override void Awake()
		{
			if (instance == null)
			{
				instance = this as T;
				DontDestroyOnLoad(gameObject);
			}
			else
				Destroy(gameObject);
		}

	}


}