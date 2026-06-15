using System;
using UnityEngine;

namespace InGame.Drawing {
	public class DrawByMouse: Draw {
		
		//==================================================Fields	
		private const int UpdateFrame = 120;
		private const float UpdateInterval = 1f / UpdateFrame;

		[SerializeField] private float _speed = 5;
		private float _timer = 0;
		private int _idx; 
		private bool _isDrawing = false;
		
		//==================================================Properties	
		public bool IsFocus { get; private set; } = false;
		
		//==================================================Methods
		public void StartDraw() {
			_timer = 0;
			_idx = 0;
			_isDrawing = true;
		}

		public void EndDraw() {
			Clear();
			_isDrawing = false;
		}

		public void Focus() {
			IsFocus = true;
			MoveCursor(Vector2.one * 0.5f);
		}

		public void CancelFocus() {
			EndDraw();
			IsFocus = false;
			MoveCursor(Vector2.one * 0.5f);
		}
		

		private void UpdateCanvas() {
			var mouseDelta = Input.mousePositionDelta;
			mouseDelta.x /= Screen.width;	
			mouseDelta.y /= Screen.height;	
			
			var temp  = GetCursor() + mouseDelta * _speed;
			MoveCursor(temp);

			if (!_isDrawing) return;
			
			_timer += Time.deltaTime;
			var curFrame = (int)Math.Ceiling(_timer / UpdateInterval);
			if (_idx == curFrame) return;
			
			DrawUpdate();
			_idx = curFrame;		
		}
		
		//==================================================Unity	
		protected override void Awake() {
			base.Awake();
			Cursor.visible = false;
		}
		
		private void Update() {
			if (Input.GetKeyDown(KeyCode.F)) {
				if(!IsFocus) Focus();
				else CancelFocus();
			}
			
			if (!IsFocus) return;
			
			if (Input.GetKeyDown(KeyCode.Mouse0)) StartDraw();
			if (Input.GetKeyUp(KeyCode.Mouse0)) {
				CancelFocus();
				//EndDraw();
			}

			UpdateCanvas();		
		}
	}
}