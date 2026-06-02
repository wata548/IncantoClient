using BVH;
using System;
using System.Collections.Generic;

namespace Networking {
	public class PlayerPacketData:PacketData {
		public Vector Pos { get; set; } = new();
		public Vector Velocity { get; set; } = new();
		public int Hp{ get; set; }
		public int Mp{ get; set; }
		
		public override IEnumerable<byte> GetBytes() {
			var result = new List<byte>();
			result.AddRange(base.GetBytes());
			result.AddRange(Pos.GetBytes());
			result.AddRange(Velocity.GetBytes());
			result.AddRange(BitConverter.GetBytes(Hp));
			result.AddRange(BitConverter.GetBytes(Mp));
			return result;
		}

		public PlayerPacketData(){}
		public PlayerPacketData(byte[] pBytes, ref int pStart)
			:base(pBytes, ref pStart){
			Pos = new Vector(pBytes, ref pStart);
			Velocity = new Vector(pBytes, ref pStart);
			Hp = GetInt(pBytes, ref pStart);
			Mp = GetInt(pBytes, ref pStart);
		}
	}

}