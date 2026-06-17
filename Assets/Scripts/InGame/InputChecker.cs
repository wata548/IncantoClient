using System;
using System.Collections.Generic;
using Networking;
using UnityEngine;

namespace InGame {
	public class InputChecker {

		//==================================================Proerperties	
		public InputFlags InputKeys { get; private set; }
		
		//==================================================Fields	
		private readonly IReadOnlyDictionary<InputFlags, InputType> _inputs =
			new Dictionary<InputFlags, InputType> {
				{ InputFlags.Focus, new(KeyCode.F, InputType.ClickType.Down) },
				{ InputFlags.Shoot, new(KeyCode.Mouse0, InputType.ClickType.Down) },
				{ InputFlags.Forward, new(KeyCode.W) },
				{ InputFlags.Backward, new(KeyCode.S) },
				{ InputFlags.Left, new(KeyCode.A) },
				{ InputFlags.Right, new(KeyCode.D) },
				{ InputFlags.Jump, new(KeyCode.Space) },
				{ InputFlags.FocusEnd, new(KeyCode.Mouse0, InputType.ClickType.Up) },
			};
		
		//==================================================Methods	
		public void ChangeKey(InputFlags pFlag, KeyCode pCode) {
			if (!_inputs.TryGetValue(pFlag, out var type)) return;
			type.ChangeKey(pCode);
		}

		public void Refresh() => InputKeys = InputFlags.None;
		
		public void Update() {
			foreach (var (key, data) in _inputs) {
				if (data.IsClick())
					InputKeys |= key;
			}
		}
		
		//==================================================SubClasses
		private class InputType {
			public enum ClickType {
				Up, Down, Press
			} 
			
			public KeyCode Code { get; private set; }
			public readonly ClickType Type;

			public InputType(KeyCode pCode, ClickType pClickType = ClickType.Press) =>
				(Code, Type) = (pCode, pClickType);

			public void ChangeKey(KeyCode pCode) =>
				Code = pCode;

			public bool IsClick() {
				switch (Type) {
					case ClickType.Press: return Input.GetKey(Code);
					case ClickType.Down: return Input.GetKeyDown(Code);
					case ClickType.Up: return Input.GetKeyUp(Code);
				}

				return false;
			}
		}
	}
}