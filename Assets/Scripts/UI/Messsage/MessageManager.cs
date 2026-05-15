using System.Collections.Generic;
using Auth;
using Extension;
using Extension.Test;
using UnityEngine;

namespace UI.Messsage {
	[RequireComponent(typeof(RectTransform))]
	public class MessageManager: MonoSingleton<MessageManager> {
		[SerializeField] private int _limit;
		[SerializeField] private float _interval = 0.1f;
		[SerializeField] private Message _messagePrefab;
		private MessagePool _pool;
		protected override bool IsNarrowSingleton => true;

		public void Add(Result pResult) {
			_pool.Add(pResult);
		}

		private void Awake() {

		

			var height = 1f / (_limit * (1 + _interval));
			
			var rect = (transform as RectTransform)!;
			_pool = new MessagePool(rect , _messagePrefab, height, _interval * height);
		}


	}
}