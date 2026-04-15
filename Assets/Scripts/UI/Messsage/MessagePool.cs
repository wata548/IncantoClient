using Auth;
using ObjectPool;
using UnityEngine;

namespace UI.Messsage {
	public class MessagePool: ObjectPoolBase<Message, Result> {

		private RectTransform _parent;
		private Message _prefab;
		private float _height;
		private float _interval;

		public MessagePool(RectTransform pParent, Message pPrefab, float pHeight, float pInterval) {
			_parent = pParent;
			_prefab = pPrefab;
			_height = pHeight;
			_interval = pInterval;
		}
		
		protected override Message GenerateInstance() {
			return Object.Instantiate(_prefab, _parent);
		}

		protected override void Update(Message pObj, int pIdx) {
			pObj.UpdatePos(pIdx * (_height + _interval), _height);
		}
	}
}