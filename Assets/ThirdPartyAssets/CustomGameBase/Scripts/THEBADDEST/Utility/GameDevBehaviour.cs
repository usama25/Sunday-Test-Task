using System;
using UnityEngine;


namespace THEBADDEST
{


	public abstract class GameDevBehaviour : MonoBehaviour
	{

		private Collider  collider;
		private Rigidbody rigidbody;
		private Animator  animator;
		public Collider Collider
		{
			get
			{
				if (collider == null)
					collider = this.GetComponentFirst<Collider>();
				return collider;
			}
		}
		public Rigidbody Rigidbody
		{
			get
			{
				if (rigidbody == null)
					rigidbody = this.GetComponentFirst<Rigidbody>();
				return rigidbody;
			}
		}
		public Animator Animator
		{
			get
			{
				if (animator == null)
					animator = this.GetComponentFirst<Animator>();
				return animator;
			}
		}
		public event Action OnInit;
		public event Action OnUpdate;
		public event Action OnFixedUpdate;

		protected virtual void Start()
		{
			OnInit?.Invoke();
		}

		protected virtual void Update()
		{
			OnUpdate?.Invoke();
		}

		protected virtual void FixedUpdate()
		{
			OnFixedUpdate?.Invoke();
		}

	}


}