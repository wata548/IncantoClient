using System;

namespace ObjectPool {
	public interface IObject<T> {
		public bool IsExist { get; }
		public void Init(Action<IObject<T>> pPushPool);
		public void Set(T pArg);
		public void Hide();
		public void OnAppear();
		public void OnHide();
	}
}