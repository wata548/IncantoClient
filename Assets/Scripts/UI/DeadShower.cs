using InGame.Map;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	[RequireComponent(typeof(Image))]
	public class DeadShower: MonoBehaviour {
		
		private Image _img;
		private bool _inited = false; 

		private void Show(int _) {
			_img.enabled = true;
		}

		private void Awake() {
			_img = GetComponent<Image>();
		}

		private void Update() {
			if (_inited) return;
			if (!Map.Instance || !Map.Instance.Player)
				return;
			Map.Instance.Player.OnDeath += Show;
		}
	}
}