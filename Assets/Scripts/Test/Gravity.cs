using System.Linq;
using BVH;
using Extensions;
using Networking;
using UnityEngine;

namespace Test {
	public class Gravity: MonoBehaviour {
		private const int FrameCnt = 30;
		private const float UpdateTerm = 1f / FrameCnt;
		private Vector _velocity = new();
		private float _remainTime = 0;
		private DataModule _module = new();
		private Vector3 _halfHeight => new(0, transform.localScale.y / 2f, 0);

		private void DataSend() {
			var rotation = transform.rotation.eulerAngles;
			var packet = new MoveData {
				Command = PacketCommand.Move,
				Id = -1,
				Input = default,
				IsPainting = false,
				MouseDelta = new(),
				Pos = (transform.position - _halfHeight).ToCustomVector(),
				Velocity = _velocity,
				Rotation = rotation.ToCustomVector()
			};
			var data = packet.GetBytes().ToArray();
			_module.Send(data);	
		}

		private void DataReceive(PacketData pPacket) {
			if (pPacket.Command != PacketCommand.Move)
				return;
			var moveData = (pPacket as MoveData)!;
			var pos = moveData.Pos.ToUnityVector();
			pos += _halfHeight;
			transform.position = pos;
			_velocity = moveData.Velocity;
			transform.rotation = Quaternion.Euler(moveData.Rotation.ToUnityVector());
		}

		private void Awake() {
			_module.OnReceive += DataReceive;
		}
		
		private void Update() {
			_module.Update();
			
			Debug.DrawRay(transform.position - _halfHeight, _velocity.ToUnityVector() * 1 / 30f);
			_remainTime -= Time.deltaTime;
			if (_remainTime > 0)
				return;

			_remainTime = UpdateTerm;
			DataSend();		
		}
	}
}