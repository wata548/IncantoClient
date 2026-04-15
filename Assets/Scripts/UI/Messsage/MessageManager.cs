using System.Collections.Generic;
using Auth;
using Extension.Test;
using UnityEngine;

namespace UI.Messsage {
	[RequireComponent(typeof(RectTransform))]
	public class MessageManager: MonoBehaviour {
		[SerializeField] private int _limit;
		[SerializeField] private float _interval = 0.1f;
		[SerializeField] private Message _messagePrefab;
		private MessagePool _pool;

		public void Add(Result pResult) {
			_pool.Add(pResult);
		}

		[TestMethod]
		public void Add(string pContext) => _pool.Add(new(Status.Success, pContext));
		
		private void Awake() {
			var height = 1f / (_limit * (1 + _interval));
			
			var rect = (transform as RectTransform)!;
			_pool = new MessagePool(rect , _messagePrefab, height, _interval * height);
		}


	}
}