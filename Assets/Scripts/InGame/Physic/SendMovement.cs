using System;
using System.Linq;
using DefaultNamespace;
using Extensions;
using InGame.Drawing;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	public class SendMovement: ReceiveMovement {

		//==================================================||Fields	
		private float _remainTime = 0;
		protected DrawByMouse _canvas;
		
		//==================================================||Properties	
		protected virtual InputFlags GetInput() => default;
        
		//==================================================||Methods	
		
		protected virtual void OnSend(){}
		
		private void DataSend() {
			if (IsDead)
				return;
			var packet = new MoveData {
				Command = PacketCommand.Move,
				Id = Idx,
				Input = GetInput(),
				Yaw = Yaw * Mathf.Deg2Rad,
				Pitch = Pitch * Mathf.Deg2Rad,
				MouseDelta = _canvas.Pool.ToArray()
			};
			OnSend();
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

		protected virtual void Awake() {
			_canvas = FindAnyObjectByType<DrawByMouse>();
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