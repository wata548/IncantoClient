using System;
using System.Linq;
using BVH;
using Extensions;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	public class ReceiveMovement: MonoBehaviour {
		//==================================================||Fields	
		
		protected Vector _velocity = new();
		protected int _id;
		private const float RotationTrashHold = 10;
		
		//==================================================Properties	
		//0 ~ 360
		protected virtual float RotationY {
			get => transform.rotation.eulerAngles.y;
			set {
				var rotation = transform.rotation.eulerAngles;
				rotation.y = value;
				transform.rotation = Quaternion.Euler(rotation);
			}
		}

		//==================================================||Methods	

		public void Init(int pId) {
			_id = pId;
			LogicConnection.Instance.OnReceiveInGame += DataReceive;
		}
		
		private void DataReceive(PacketData pPacket) {
			if (_id != pPacket.Id)
				return;
			if (pPacket.Command != PacketCommand.Move)
				return;
			if (pPacket is not MoveData moveData)
				return;
			var pos = moveData.Pos.ToUnityVector();
			transform.position = pos;
			_velocity = moveData.Velocity;

			if (moveData.Rotation != -1) {
				var rotation = moveData.Rotation * 180 / MathF.PI;
				Debug.Log($"Fix rotation: {RotationY} -> {rotation}");
				RotationY = rotation;
			}
		}

		//==================================================||Unity	
	}
}