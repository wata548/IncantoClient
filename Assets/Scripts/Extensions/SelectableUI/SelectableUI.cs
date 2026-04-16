using System;
using Extension.Test;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Extension.SelectableUI {
	
	[RequireComponent(typeof(Selectable))]
	public class SelectableUI: MonoBehaviour {
		
		[field: SerializeField] public string Tag { get; private set; }

#if UNITY_EDITOR
		public void SetTag(string pTag) =>
			Tag = pTag;
		
		[TestMethod]
		private void FindTag() {
			var parent = transform.parent;
			while (parent != null) {
				var modal = parent.GetComponent<ModalBase>();
				var selectable = parent.GetComponent<SelectableUI>();
				var tag = "";
				if (modal != null)
					tag = modal!.Tag;
				if (selectable != null)
					tag = selectable!.Tag;
				Tag = tag;

				if (!String.IsNullOrWhiteSpace(Tag))
					break;
				
				parent = parent.parent;
			}
		}
#endif
		
		private void Awake() {
			SelectableUIManager.Instance.Set(Tag, this);
		} 
	}
}