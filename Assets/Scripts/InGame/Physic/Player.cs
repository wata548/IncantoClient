using System;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	public class Player: SendMovement {
		
		//==================================================Properties	
		[SerializeField] private float _sensibility = 1f;
        
		protected Camera _camera;
		private InputChecker _input = new();
        
		//==================================================||Properties 
		protected override float Yaw {
			get => _camera.transform.rotation.eulerAngles.y;
			set {
				var rotation = _camera.transform.rotation.eulerAngles;
				rotation.y = value;
				_camera.transform.rotation = Quaternion.Euler(rotation);
			}
		}

		protected override float Pitch {
			get => -_camera.transform.rotation.eulerAngles.x;
			set {
				var rotation = _camera.transform.rotation.eulerAngles;
				var temp = value;
				if (temp >= 180)
					temp -= 360;
				rotation.x = Math.Clamp(temp, -70f, 70f);
				_camera.transform.rotation = Quaternion.Euler(rotation);       
			}
		}

		//==================================================Methods	
		protected override InputFlags GetInput() => _input.InputKeys;

		private void CameraPositionUpdate() {
			var delta = Input.mousePositionDelta * _sensibility;
			var rotation = _camera.transform.rotation.eulerAngles;
			Pitch = rotation.x - delta.y;
			Yaw = rotation.y + delta.x;
		}

		protected override void OnSend() =>
			_input.Refresh();

		//==================================================||Unity 
		protected override void Update() {
			CameraPositionUpdate();
			_input.Update();
			base.Update();

#if UNITY_EDITOR
			var yaw = Yaw * Mathf.Deg2Rad;
			var pitch = Pitch * Mathf.Deg2Rad;
			var l = MathF.Cos(pitch);
			var z = MathF.Cos(yaw) * l;
			var x = MathF.Sin(yaw) * l;
			var y = MathF.Sin(pitch);
			var direction = new Vector3(x, y, z) * 100;
			Debug.DrawRay(transform.position + Vector3.up * 2, direction);
#endif
		}
		
		private void Awake() {
			_camera = Camera.main!;
		}
	}
}