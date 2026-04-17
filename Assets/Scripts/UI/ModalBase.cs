using System;
using System.Collections.Generic;
using Extension.SelectableUI;
using Extension.Test;
using UnityEngine;

namespace UI {
	public abstract class ModalBase: MonoBehaviour {
		
		//==================================================|| Properties	
		[field: SerializeField] public string Tag { get; protected set; }
		public bool IsActive { get; private set; }
		
		//==================================================|| Fields	
		[SerializeField] private GameObject _panel;
		private static Dictionary<string, ModalBase> _modals = new(); 
		//==================================================|| Methods

		public static ModalBase GetModal(string pModal) {
			_modals.TryGetValue(pModal, out var modal);
			return modal;
		}

		public static void Clear() =>
			SelectableUIManager.Instance.Clear(v => GetModal(v)?.ClearProcess());
		
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

		private void ClearProcess() {
			IsActive = false;
			_panel.SetActive(false);
		} 

		protected virtual void Awake() {
			IsActive = _panel.activeSelf;
			_modals.Add(Tag, this);
		}
	}
}