using System;
using UnityEngine;


namespace THEBADDEST
{


	public abstract class GameDevObject : ScriptableObject
	{
		

		public event Action<GameDevBehaviour> OnInit;
		public event Action OnUpdate;
		public event Action OnFixedUpdate;
		

	}


}