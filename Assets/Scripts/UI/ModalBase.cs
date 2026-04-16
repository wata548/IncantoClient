using Extension.SelectableUI;
using Extension.Test;
using UnityEngine;

namespace UI {
	public abstract class ModalBase: MonoBehaviour {
		[SerializeField] private GameObject _panel;
		public bool IsActive { get; private set; }
		[field: SerializeField] public string Tag { get; protected set; }

#if UNITY_EDITOR
		[TestMethod]
		protected void SetSelectableTag() {
			if (string.IsNullOrWhiteSpace(Tag))
				Tag = gameObject.name;
			var uis = GetComponentsInChildren<Extension.SelectableUI.SelectableUI>();
			foreach (var ui in uis) {
				ui.SetTag(Tag);
			}
			Debug.Log($"{uis.Length} elements updated");
		}
#endif
		
		public virtual void Show() {
			IsActive = true;
			_panel.SetActive(true);
			SelectableUIManager.Instance.Open(Tag);
		}
		
		public virtual void Hide() {
			IsActive = false;
			_panel.SetActive(false);
			SelectableUIManager.Instance.Close();
		}
	}
}