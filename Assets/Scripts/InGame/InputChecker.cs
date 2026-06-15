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
				{ InputFlags.Focus, new(KeyCode.F, true) },
				{ InputFlags.Shoot, new(KeyCode.Mouse0, true) },
				{ InputFlags.Forward, new(KeyCode.W) },
				{ InputFlags.Backward, new(KeyCode.S) },
				{ InputFlags.Left, new(KeyCode.A) },
				{ InputFlags.Right, new(KeyCode.D) },
				{ InputFlags.Jump, new(KeyCode.Space) },
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
			public KeyCode Code { get; private set; }
			public readonly bool ClickType;

			public InputType(KeyCode pCode, bool pClickType = false) =>
				(Code, ClickType) = (pCode, pClickType);

			public void ChangeKey(KeyCode pCode) =>
				Code = pCode;		
			
			public bool IsClick() =>
				ClickType 
					? Input.GetKeyDown(Code) 
					: Input.GetKey(Code);
		}
	}
}