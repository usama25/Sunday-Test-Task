using System;
using System.Collections;
using System.Collections.Generic;


namespace THEBADDEST
{


	public class Enumerator<T> : IEnumerator<T>
	{

		readonly T[] elements;
		int          position = -1;

		public Enumerator(T[] elements)
		{
			this.elements = elements;
		}

		public bool MoveNext()
		{
			position++;
			return position < elements.Length;
		}

		public void Reset()
		{
			position = -1;
		}

		public T Current
		{
			get
			{
				try
				{
					return elements[position];
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}

		object IEnumerator.Current => Current;

		void IDisposable.Dispose()
		{
			
		}
		

	}


}