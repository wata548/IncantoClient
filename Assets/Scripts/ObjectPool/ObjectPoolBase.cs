using System.Collections.Generic;
using System.Linq;

namespace ObjectPool {
	public abstract class ObjectPoolBase<TO, TD> where TO: class, IObject<TD> {
		private readonly Queue<TO> _objectPool = new();
		private List<TO> _curObjects = new();
		protected IEnumerable<TO> GetObjects => _curObjects;

		protected abstract TO GenerateInstance();
		
		public void Add(TD pData) {
			var target = default(TO);
			if (_objectPool.Count == 0) {
				target = GenerateInstance();
				target.Init(obj => AddToPool(obj as TO));
			}
			else 
				target = _objectPool.Dequeue();
			
			target.Set(pData);
			target.OnAppear();
			_curObjects.Add(target);
			Update();
			return;
		}

		private void AddToPool(TO pAdd) {
			_objectPool.Enqueue(pAdd);
			Update();
		}

		private void Update() {
			var idx = 0;
			_curObjects = _curObjects.Where(obj => obj.IsExist).ToList();
			foreach (var obj in _curObjects) {
				Update(obj, idx);
				idx++;
			}
		}

		protected abstract void Update(TO pObj, int pIdx);
	}
}