using System;
using System.Collections.Generic;
using System.Linq;
using BVH;
using Extensions;
using Networking;
using UnityEngine;

namespace InGame.Drawing {
	public class DrawByMouse: Draw {
		
		//==================================================Fields	
		private const int UpdateFrame = 30 * MoveData.MouseMaxUpdateTerm;
		private const float UpdateInterval = 1f / UpdateFrame;
		

		[SerializeField] private float _speed = 5;
		private float _timer = 0;
		private int _idx; 
		private bool _isDrawing = false;
		private readonly Vector[] _pool = new Vector[MoveData.MouseMaxUpdateTerm];
		private int _poolIdx = 0;
		
		//==================================================Properties	
		public bool IsFocus { get; private set; } = false;

		public Vector[] Pool {
			get {
				for (; _poolIdx < MoveData.MouseMaxUpdateTerm; _poolIdx++) {
					_pool[_poolIdx] = new(-1, -1, -1);
				}
				_poolIdx = 0;
				return _pool;
			}
		}
		
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
			_pool[_poolIdx] = temp.ToCustomVector();
			_poolIdx++;
			_poolIdx %= MoveData.MouseMaxUpdateTerm;
			
			DrawUpdate();
			_idx = curFrame;		
		}
		
		//==================================================Unity	
		protected override void Awake() {
			base.Awake();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
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