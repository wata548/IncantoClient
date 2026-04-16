using System.Collections;
using Auth;
using ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Messsage {
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(Button))]
	public class Message: ObjectBase<Result> {
		[SerializeField] private Text _text;
		[SerializeField] private Outline _outline;
		private RectTransform _rect;

		public override void OnHide() {
			base.OnHide();
			gameObject.SetActive(false);
		}
		
		public override void Set(Result pData) {
			gameObject.SetActive(true);
			_outline.effectColor = pData.Status == Status.Success
				? Color.green
				: Color.red;
			_text.text = $"{pData.Status}: {pData.Context}";
			
			StartCoroutine(Wait());

			IEnumerator Wait() {
				yield return new WaitForSeconds(3f); 
				Hide();
			}
		}

		public void UpdatePos(float pPos, float pLength) {
			var min = _rect.anchorMin;
			var max = _rect.anchorMax;
			min.y = pPos;
			max.y = pPos + pLength;
			
			_rect.anchorMin = min;
			_rect.anchorMax = max;
		}

		private void Awake() {
			GetComponent<Button>().onClick.AddListener(Hide);
			_outline = GetComponent<Outline>();
			_rect = (transform as RectTransform)!;
		}
	}
}