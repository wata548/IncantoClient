using System.Linq;
using BVH;
using Extensions;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	public class ApplyMovement: MonoBehaviour {
		//==================================================||Fields	
		
		protected static DataModule _module = new();
		protected Vector _velocity = new();
		
		//==================================================||Methods	
		protected virtual InputFlags GetInput() => default;
		
		private void DataReceive(PacketData pPacket) {
			if (pPacket.Command != PacketCommand.Move)
				return;
			var moveData = (pPacket as MoveData)!;
			var pos = moveData.Pos.ToUnityVector();
			transform.position = pos;
			_velocity = moveData.Velocity;
			transform.rotation = Quaternion.Euler(0, moveData.Rotation, 0);
		}

		//==================================================||Unity	
		private void Awake() {
			_module.OnReceive += DataReceive;
		}
		
		protected virtual void Update() {
			_module.Update();
		}
	}
}