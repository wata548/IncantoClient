using System;
using System.Linq;
using DefaultNamespace;
using Extensions;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	public class SendMovement: ReceiveMovement {

		//==================================================||Fields	
		private float _remainTime = 0;
		
		//==================================================||Properties	
		protected virtual InputFlags GetInput() => default;
        
		//==================================================||Methods	
		
		private void DataSend() {
			var packet = new MoveData {
				Command = PacketCommand.Move,
				Id = _id,
				Input = GetInput(),
				Pitch = Pitch * (MathF.PI / 180f),
				Yaw = Yaw
			};
			var data = packet.GetBytes().ToArray();
			LogicConnection.Instance.Send(data);	
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
			Gizmos.DrawWireSphere(
				transform.position + _velocity.ToUnityVector() * ServerSetting.UpdateTerm, 
				transform.localScale.x / 2f
			);
		}
#endif
	}
}