using System.Collections;
using Extension.Test;
using InGame.Map;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	[RequireComponent(typeof(Image))]
	public class DamageShower: MonoBehaviour {

		[SerializeField] private float Duration = 0.5f;
		private Material _mat;
		private Coroutine _coroutine;
		private bool _inited = false; 

		[TestMethod]
		private void Show(int _) {
			if(_coroutine != null)
				StopCoroutine(_coroutine);
			_coroutine = StartCoroutine(ShowCoroutine());
			IEnumerator ShowCoroutine() {
				var process = 1f;
				while (process > 0) {
					process -= Time.deltaTime;
					_mat.SetFloat("_Percentage", process);
					yield return null;
				}
			}
		}

		private void Awake() {
			_mat = GetComponent<Image>().material;
		}

		private void Update() {
			if (_inited) return;
			if (!Map.Instance || !Map.Instance.Player)
				return;
			Map.Instance.Player.OnDamaged += Show;
		}
	}
}