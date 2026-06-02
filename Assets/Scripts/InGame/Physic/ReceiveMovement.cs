using System;
using BVH;
using Extensions;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	//[RequireComponent(typeof(Rigidbody))]
	public class ReceiveMovement: MonoBehaviour {
		//==================================================||Fields	

		//protected Rigidbody _rigid;
		protected Vector _velocity = new();
		protected int _id;
		private const float GravityScale = -23.75f;
		
		//==================================================Properties	
		//0 ~ 360
		protected virtual float Pitch {
			get => transform.rotation.eulerAngles.y;
			set {
				var rotation = transform.rotation.eulerAngles;
				rotation.y = value;
				transform.rotation = Quaternion.Euler(rotation);
			}
		}

		protected virtual float Yaw { get; set;}

		//==================================================||Methods	

		public void Init(int pId) {
			_id = pId;
			LogicConnection.Instance.OnReceiveInGame += DataReceive;
		}
		
		private void DataReceive(PacketData pPacket) {
			//_rigid ??= GetComponent<Rigidbody>();
			
			if (_id != pPacket.Id)
				return;
			if (pPacket.Command != PacketCommand.PlayerData)
				return;
			if (pPacket is not PlayerPacketData player)
				return;
			var pos = player.Pos.ToUnityVector();
			transform.position = pos;
			_velocity = player.Velocity;
			//_rigid.linearVelocity = _velocity.ToUnityVector();

			/*if (moveData.Rotation != -1) {
				var rotation = moveData.Rotation * 180 / MathF.PI;
				Debug.Log($"Fix rotation: {RotationY} -> {rotation}");
				RotationY = rotation;
			}*/
		}

		//==================================================||Unity	

		protected virtual void Update() {
			//_velocity.Y += GravityScale * Time.deltaTime;
			var velo = _velocity;
			velo.Y = MathF.Max(0, velo.Y);
			transform.position += velo.ToUnityVector() * Time.deltaTime;
		}
	}
}