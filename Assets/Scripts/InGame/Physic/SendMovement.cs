using System.Linq;
using DefaultNamespace;
using Extensions;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	public class SendMovement: ApplyMovement {

		//==================================================||Fields	
		private InputChecker _input = new();
		private float _remainTime = 0;
		
		//==================================================||Properties	
		protected override InputFlags GetInput() => _input.GetInput();
        
		//==================================================||Methods	
		private void DataSend() {
			Debug.Log(GetInput());
			var rotation = transform.rotation.eulerAngles;
			var packet = new MoveData {
				Command = PacketCommand.Move,
				Id = -1,
				Input = GetInput(),
				IsPainting = false,
				MouseDelta = new(),
				Pos = transform.position.ToCustomVector(),
				Radius = transform.localScale.x / 2f,
				Velocity = _velocity,
				Rotation = rotation.y
			};
			var data = packet.GetBytes().ToArray();
			_module.Send(data);	
		}

		//==================================================||Unity	
		protected override void Update() {
			base.Update();
			
			_remainTime -= Time.deltaTime;
			if (_remainTime > 0)
				return;
            
			_remainTime = ServerSetting.UpdateTerm;
			DataSend();
		}
        
#if UNITY_EDITOR
		private void OnDrawGizmos() {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(
				transform.position + _velocity.ToUnityVector() * ServerSetting.UpdateTerm, 
				transform.localScale.x / 2f
			);
		}
#endif
	}
}