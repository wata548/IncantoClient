using System;
using UnityEngine;

namespace ObjectPool {
	public abstract class ObjectBase<T>: MonoBehaviour, IObject<T> {
		protected Action<IObject<T>> _pushPool;

		public bool IsExist { get; private set; }

		public virtual void Init(Action<IObject<T>> pPushPool) {
			_pushPool = null;
			_pushPool += obj => obj.OnHide();
			_pushPool += pPushPool;
		}

		public abstract void Set(T pArg);

		public virtual void Hide() =>
			_pushPool?.Invoke(this);
		
		public virtual void OnAppear() {
			IsExist = true;
		}

		public virtual void OnHide() {
			IsExist = false;
		}
	}
}