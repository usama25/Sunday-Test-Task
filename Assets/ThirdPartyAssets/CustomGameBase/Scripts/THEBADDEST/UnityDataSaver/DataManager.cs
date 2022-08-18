using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif


namespace THEBADDEST.DataManagement
{


	public static class DataManager
	{

		public static  bool                     isInitialized;
		public static  IDataSaver               dataSaver { get; private set; }
		private static Dictionary<string, Data> data         = new Dictionary<string, Data>();
		private static string                   dataFileName = "gamedata";

		/// <summary>
		/// initialize data system
		/// </summary>
		static void Initialize()
		{
			Debug.Log($"Data Initialized with BinaryDataSaver {Application.persistentDataPath}");
			dataSaver     = new BinaryDataSaver();
			isInitialized = true;
			Load();
		}

		/// <summary>
		/// initialize data system with desired IDataSaver type data saver
		/// </summary>
		/// <param name="dataSaver"></param>
		public static void Initialize(IDataSaver dataSaver)
		{
			DataManager.dataSaver = dataSaver;
			isInitialized         = true;
		}

		/// <summary>
		/// CanGet is used for get data if data exists
		/// </summary>
		/// <param name="key">data saved as named</param>
		/// <param name="dataObject">carries data</param>
		/// <typeparam name="T">type of data</typeparam>
		/// <returns>Return true if it finds data</returns>
		public static bool CanGet<T>(string key, out T dataObject)
		{
			if (!isInitialized)
				Initialize();
			if (data.ContainsKey(key))
			{
				dataObject = ((Data<T>) data[key]).value;
				return true;
			}

			dataObject = default(T);
			return false;
		}

		/// <summary>
		/// Contains used to check is data exist with name of key
		/// </summary>
		/// <param name="key">data saved as named</param>
		/// <returns></returns>
		public static bool Contains(string key)
		{
			if (!isInitialized)
				Initialize();
			return data.ContainsKey(key);
		}

		/// <summary>
		/// Delete saved data by key(data saved name)
		/// </summary>
		/// <param name="key"></param>
		public static void Delete(string key)
		{
			if (!isInitialized)
				Initialize();
			data.Remove(key);
		}

		/// <summary>
		/// Used to get data by key
		/// </summary>
		/// <param name="key"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Get<T>(string key)
		{
			if (!isInitialized)
				Initialize();
			if (data.ContainsKey(key))
				return ((Data<T>) data[key]).value;
			return default(T);
		}

		/// <summary>
		/// Used to saved data by key 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="dataObject"></param>
		/// <typeparam name="T"></typeparam>
		public static void Save<T>(string key, T dataObject)
		{
			if (!isInitialized)
				Initialize();
			var dataEntity = new Data<T>(dataObject);
			if (data.ContainsKey(key))
			{
				data[key] = dataEntity;
			}
			else
			{
				data.Add(key, dataEntity);
			}
		}

		/// <summary>
		/// Save Data of IDataElement 
		/// </summary>
		/// <param name="dataElement"></param>
		public static void Save(IDataElement dataElement)
		{
			if (!isInitialized)
				Initialize();
			Save(dataElement.dataTag, dataElement.SaveData());
		}

		/// <summary>
		/// Get Data of DataElement
		/// </summary>
		/// <param name="dataElement"></param>
		public static void Get(IDataElement dataElement)
		{
			if (!isInitialized)
				Initialize();
			Data data = Get<Data>(dataElement.dataTag);
			if (data != null)
				dataElement.LoadData(data);
			else
			{
				Debug.Log("Data not found");
			}
		}

		/// <summary>
		/// Save to all data to hard form
		/// </summary>
		public static void Persist()
		{
			var serializedList  = data.Keys.Select(element => new KeyValuePair<string, Data>(element, data[element]));
			var serializedArray = new DataArray<KeyValuePair<string, Data>>(serializedList.ToArray());
			dataSaver.Save(dataFileName, serializedArray);
		}


		/// <summary>
		/// Load Data from hard form 
		/// </summary>
		public static void Load()
		{
			if (!dataSaver.Contains(dataFileName))
				return;
			var serializedArray = dataSaver.Get<DataArray<KeyValuePair<string, Data>>>(dataFileName);
			foreach (var entry in serializedArray.values)
			{
				data[entry.Key] = entry.Value;
			}
		}


		#if UNITY_EDITOR
		[MenuItem("Tools/THEBADDEST/Data Manager/Delete Data File")]
		public static void DeleteAllData()
		{
			if (Application.isPlaying)
			{
				dataSaver.Delete(dataFileName);
				Debug.Log("Data Deleted!");
			}
			else
			{
				Debug.Log("Application Play Mode Required!");
			}
		}

		[MenuItem("Tools/THEBADDEST/Data Manager/Force Delete Data File")]
		public static void ForceDeleteAllData()
		{
			Initialize();
			dataSaver.Delete(dataFileName);
			Debug.Log("Data Deleted!");
		}
		#endif

	}


	public interface IDataElement
	{

		string dataTag { get; }

		Data SaveData();

		void LoadData(Data data);

	}


}