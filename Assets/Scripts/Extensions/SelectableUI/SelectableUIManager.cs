using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Extension.SelectableUI {
	public class SelectableUIManager: MonoSingleton<SelectableUIManager> {
		protected override bool IsNarrowSingleton => false;

		private readonly Dictionary<string, SortedList<Vector2, SelectableUI>> _tagUIs = new();
		private Stack<string> _tagStack = new();
		private int _idx = 0; 

		public void Set(string pTag, SelectableUI pUI) {
			var comp = Comparer<Vector2>.Create((v1, v2) =>
				Mathf.Approximately(v1.y, v2.y)
					? v1.x.CompareTo(v2.x)
					: -v1.y.CompareTo(v2.y)
			);
			
			_tagUIs.TryAdd(pTag, new(comp));
			_tagUIs[pTag].Add(pUI.transform.position, pUI);
		}

		private void Focus() {
			if (_tagStack.Count == 0)
				return;	
			var tag = _tagStack.Peek();
			var target = _tagUIs[tag].ElementAt(_idx).Value;
			var selectable = target.GetComponent<Selectable>()!;
			EventSystem.current.SetSelectedGameObject(selectable.gameObject);	
		}
		
		public void Next() {
			if (_tagStack.Count == 0)
				return;	
			
			_idx++;
			var tag = _tagStack.Peek();
			if (_idx == _tagUIs[tag].Count)
				_idx = 0;
			Focus();
		}

		public void Prev() {
			if (_tagStack.Count == 0)
				return;
			_idx--;
			var tag = _tagStack.Peek();
			if (_idx == -1)
				_idx = _tagUIs[tag].Count - 1;
			Focus();
		}
		
		public void Open(string pTag) {
			_idx = 0;
			_tagStack.Push(pTag);
			Focus();
		}

		public void Close() {
			_idx = 0;
			_tagStack.Pop();
		}

		private void OnSceneLoad() {
			_tagStack.Clear();
		}

		private void Awake() {
			SceneManager.sceneLoaded += (_, _) => OnSceneLoad();
		}

		private void Update() {
			if (!Input.GetKeyDown(KeyCode.Tab))
				return;
			if(Input.GetKey(KeyCode.LeftShift))
				Prev();
			else
				Next();
		}
	}
}