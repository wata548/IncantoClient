using System;
using UnityEngine;

namespace UI {
	public abstract class ModalBase: MonoBehaviour {
		[SerializeField] private GameObject _panel;
		public bool IsActive { get; private set; }

		public virtual void Show() {
			IsActive = true;
			_panel.SetActive(true);
		}
		public virtual void Hide() {
			IsActive = false;
			_panel.SetActive(false);
		}
	}
}