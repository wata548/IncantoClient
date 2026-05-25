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
			var moveData = (pPacket as MoveData)!;
			var pos = moveData.Pos.ToUnityVector();
			transform.position = pos;
			_velocity = moveData.Velocity;
		}

		//==================================================||Unity	
	}
}