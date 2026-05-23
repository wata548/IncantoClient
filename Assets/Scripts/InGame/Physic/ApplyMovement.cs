using System.Linq;
using BVH;
using Extensions;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	public class ApplyMovement: MonoBehaviour {
		//==================================================||Fields	
		
		protected Vector _velocity = new();
		
		//==================================================||Methods	
		
		private void DataReceive(PacketData pPacket) {
			if (pPacket.Command != PacketCommand.Move)
				return;
			var moveData = (pPacket as MoveData)!;
			var pos = moveData.Pos.ToUnityVector();
			transform.position = pos;
			_velocity = moveData.Velocity;
		}

		//==================================================||Unity	
		protected virtual void Awake() {
			LogicConnection.Instance.OnReceiveInGame += DataReceive;
		}
	}
}