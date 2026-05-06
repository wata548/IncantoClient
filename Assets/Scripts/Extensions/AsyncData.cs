using System;
using System.Threading.Tasks;

namespace Extension {
	public abstract class AsyncDataBase<T>
		where T: class
	{
		public T Value {
			get {
				if (_value != null)
					return _value;
				var data = GetData();
				if (data == null)
					return null;
				_value = CallBackTToT?.Invoke(data) ?? data;
				return _value;
			}	
		}

		private T _value = null;
		public abstract TaskStatus Status { get; }
		protected abstract T GetData();
		public event Func<T, T> CallBackTToT;
	} 
	
	public class AsyncData<TOut, TValue>: AsyncDataBase<TValue> 
		where TOut : class 
		where TValue: class {
		
		private readonly Task<TOut> _task;
		public event Func<TOut, TValue> CallBackOutToValue;

		public AsyncData(Task<TOut> pTask, Func<TOut, TValue> pCallBack) {
			_task = pTask;
			CallBackOutToValue = pCallBack;
		}

		public override TaskStatus Status => _task.Status;

		protected override TValue GetData() {
			if (!_task.IsCompleted)
				return null;
			var value = _task.Result;
			return CallBackOutToValue!(value);
		}
	}
}