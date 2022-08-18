using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;


namespace THEBADDEST.DataManagement
{


	[Serializable]
	public class Data
	{

	}

	[Serializable]
	public class Data<T> : Data
	{

		public T value;

		public Data(T value)
		{
			this.value = value;
		}

	}


	[Serializable]
	public class DataArray<T> : Data
	{

		public T[] values;

		public DataArray(params T[] values)
		{
			this.values = values;
		}

	}

	

	[Serializable]
	public class SerializableDictionary<T, T1> : Data
	{

		public T[]  keys;
		public T1[] values;

	}
	
	public class GenericPanelData : Data
	{

		public string      title;
		public string      message;
		public string      negativeText;
		public UnityAction negativeEvent;
		public bool        isPositiveButton;
		public UnityAction positiveEvent;
		public string      positiveText;

		public GenericPanelData(string title, string message, string negativeText, UnityAction negativeEvent, bool isPositiveButton, UnityAction positiveEvent, string positiveText)
		{
			this.title            = title;
			this.message          = message;
			this.negativeText     = negativeText;
			this.negativeEvent    = negativeEvent;
			this.isPositiveButton = isPositiveButton;
			this.positiveEvent    = positiveEvent;
			this.positiveText     = positiveText;
		}

	}

	public interface IStorageService
	{

		IList<Data> data { get; }

		void Save(Data dataEntity);

		T Load<T>() where T: Data ;

	}

	public class LocalStorageService : IStorageService
	{

		static LocalStorageService instance;
		public IList<Data>         data { get; }

		public static LocalStorageService GetService()
		{
			if (instance == null)
				instance = new LocalStorageService();
			return instance;
		}

		private LocalStorageService()
		{
			data = new List<Data>();
		}


		public void Save(Data dataEntity)
		{
			var index = this.data.IndexOf(dataEntity);
			if (index != -1)
			{
				data[index] = dataEntity;
				return;
			}

			this.data.Add(dataEntity);
		}

		public T Load<T>() where T : Data
		{
			return (T)data.FirstOrDefault(data1 => data1.GetType() == typeof(T));
		}

	}
}